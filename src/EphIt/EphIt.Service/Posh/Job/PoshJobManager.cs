using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using EphIt.Service.Posh.Job;
using EphIt.Service.Posh;
using System.Management.Automation;
using System.Linq;
using Serilog;
using EphIt.BL.JobManager;
using EphIt.BL.Automation;
using System.Security.Policy;
using Microsoft.Extensions.Configuration;
using EphIt.Db.Enums;

namespace EphIt.Service.Posh.Job
{
    public class PoshJobManager : IPoshJobManager
    {
        private ConcurrentDictionary<Guid, bool> jobsWithErrors = new ConcurrentDictionary<Guid, bool>();
        private ConcurrentQueue<PoshJob> pendingJobQueue = new ConcurrentQueue<PoshJob>();
        private ConcurrentQueue<ProblemJob> problemJobs = new ConcurrentQueue<ProblemJob>();
        private ConcurrentQueue<PoshJob> retryJobs = new ConcurrentQueue<PoshJob>();
        private ConcurrentDictionary<Guid, PoshJob> runningJobs = new ConcurrentDictionary<Guid, PoshJob>();
        private IPoshManager _poshManager;
        private IAutomationHelper automationHelper;
        //private IStreamHelper _streamHelper;

        //private EphIt.BL.JobManager.IJobManager _jobManager;
        public PoshJobManager(IPoshManager poshManager, IConfiguration config)
        {
            //_jobManager = jobManager;
            //_streamHelper = streamHelper;
            _poshManager = poshManager;
            automationHelper = new AutomationHelper();
            automationHelper.SetServer(config.GetSection("ServerInfo:WebServer").Value);
            automationHelper.SetPort(Int32.Parse(config.GetSection("ServerInfo:Port").Value));
        }
        public void ProcessRunningJobs()
        {
            foreach(var runningJob in runningJobs)
            {
                var jobTask = runningJob.Value.RunningJob;
                var poshInstance = runningJob.Value.PoshInstance;
                if(jobTask.Status == TaskStatus.RanToCompletion)
                {
                    _poshManager.RetirePowerShell(poshInstance);
                    RemoveRunningJob(runningJob);
                    string withErrors = "";
                    bool hasErrored = false;
                    jobsWithErrors.TryRemove(runningJob.Key, out hasErrored);
                    if(hasErrored)
                    {
                        withErrors = " with errors";
                    } 
                    Log.Information($"Job {runningJob.Key} has completed{withErrors}.");

                    string url = automationHelper.GetUrl() + $"/api/Job/{runningJob.Value.JobUID}/Finish?error={hasErrored.ToString()}";
                    automationHelper.PostWebCall(url, null);
                }
                if (jobTask.Status == TaskStatus.Faulted)
                {
                    string url = automationHelper.GetUrl() + $"/api/Job/{runningJob.Value.JobUID}/Finish?error=True";
                    automationHelper.PostWebCall(url, null);
                    Log.Warning($"Job {runningJob.Key} faulted.");
                }
            }
        }
        public void FaultJob(Guid jobId)
        {
            jobsWithErrors.TryAdd(jobId, true);
        }
        public void RemoveRunningJob(KeyValuePair<Guid, PoshJob> runningJob)
        {
            PoshJob result;
            runningJobs.Remove(runningJob.Key, out result);
            if (result == null) //cound not remove.  Is it already gone?
            {
                runningJobs.TryGetValue(runningJob.Key, out result);
            }
            else
            {
                return;
            }
            if (result != null) //its still there
            {
                runningJobs.Remove(runningJob.Key, out result);
            }
            if (result == null) //could not remove
            {
                //i dont know what to do here.
            }
            return;
        }
        public void QueuePendingJob(PoshJob job)
        {
            pendingJobQueue.Enqueue(job);
        }
        public PoshJob DequeuePendingJob()
        {
            PoshJob result;
            pendingJobQueue.TryDequeue(out result);
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public bool HasPendingJob()
        {
            return !pendingJobQueue.IsEmpty;
        }
        public void StartPendingJob()
        {   
            
            if(!HasPendingJob())
            {
                return;
            }
            /*if(!_poshManager.RunspaceAvailable())
            {
                return;
            }*/
            PowerShell ps = _poshManager.GetPowerShell();
            if(ps == null)
            {
                Log.Warning("All PowerShell instances are in use.");
                return;
            }
            PoshJob pendingJob;
            pendingJobQueue.TryDequeue(out pendingJob);
            if(pendingJob == null)
            {
                return;
            }
            try
            {
                ps.AddScript(pendingJob.Script);
                pendingJob.PoshInstance = ps;
                pendingJob = ConfigureStreams(pendingJob);
                PoshJob runningJob = RunJob(pendingJob);
                if (runningJob == null)
                {
                    retryJobs.Enqueue(pendingJob);
                }
                else
                {
                    bool success = runningJobs.TryAdd(pendingJob.JobUID, runningJob);
                    if(!success) 
                    {
                        //this is bad and hopefully never happens.
                        Log.Error("Unable to queue the running job, this job will not be monitored.");
                    }
                    else
                    {
                        string url = automationHelper.GetUrl() + $"/api/Job/{runningJob.JobUID}/Start";
                        automationHelper.PostWebCall(url, null);
                    }
                }
            }
            catch (Exception e)
            {
                ProblemJob problemJob = new ProblemJob();
                problemJob.Exception = e;
                problemJob.Job = pendingJob;
                if(!problemJobs.Contains(problemJob))
                {
                    problemJobs.Enqueue(problemJob);
                }
            }            
        }
        public void RecordStream(PoshJob poshJob, object sender, DataAddedEventArgs e)
        {
            var type = sender.GetType();
            LogPostParameters logPostParameters = new LogPostParameters();
            InformationalRecord record;

            if (type == typeof(PSDataCollection<VerboseRecord>))
            {
                record = ((PSDataCollection<VerboseRecord>)sender)[e.Index];
                logPostParameters.message = record.Message;
                logPostParameters.level = JobLogLevelEnum.Verbose;
            }
            if (type == typeof(PSDataCollection<DebugRecord>))
            {
                record = ((PSDataCollection<DebugRecord>)sender)[e.Index];
                logPostParameters.message = record.Message;
                logPostParameters.level = JobLogLevelEnum.Debug;
            }
            if (type == typeof(PSDataCollection<ErrorRecord>))
            {
                FaultJob(poshJob.JobUID);
                logPostParameters.level = JobLogLevelEnum.Error;
                var errorRecord = ((PSDataCollection<ErrorRecord>)sender)[e.Index];
                logPostParameters.Exception = errorRecord.Exception.Message;
                logPostParameters.message = errorRecord.ScriptStackTrace; //this is where the error occured. Not sure if there is a better message for error streams
            }
            if (type == typeof(PSDataCollection<WarningRecord>))
            {
                logPostParameters.level = JobLogLevelEnum.Warning;
                record = ((PSDataCollection<WarningRecord>)sender)[e.Index];
                logPostParameters.message = record.Message;
            }
            logPostParameters.jobUid = poshJob.JobUID;
            string url = automationHelper.GetUrl() + $"/api/Log/";
            automationHelper.PostWebCall(url, logPostParameters);
        }
        public void RecordOutput(PoshJob poshJob, object sender, DataAddedEventArgs e)
        {
            var record = ((PSDataCollection<PSDataCollection<PSObject>>)sender)[e.Index];
            Log.Information($"{poshJob.JobUID} Output: Type - {record[0].TypeNames[0]} Value - {record[0].BaseObject}");
        }

        public PoshJob ConfigureStreams(PoshJob poshJob)
        {
            //capture stream output
            poshJob.PoshInstance.Streams.Verbose.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                RecordStream(poshJob, sender, e);
            };
            poshJob.PoshInstance.Streams.Debug.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                RecordStream(poshJob, sender, e);
            };
            poshJob.PoshInstance.Streams.Error.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                RecordStream(poshJob, sender, e);
            };
            poshJob.PoshInstance.Streams.Information.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                RecordStream(poshJob, sender, e);
            };
            poshJob.PoshInstance.Streams.Warning.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                RecordStream(poshJob, sender, e);
            };
            return poshJob;
        }
        public PoshJob RunJob(PoshJob poshJob)
        {
            if (poshJob.Parameters != null && poshJob.Parameters.Count != 0)
            {
                poshJob.PoshInstance.AddParameters(poshJob.Parameters);
            }

            poshJob.RunningJob = StartScript(poshJob);
            Log.Information($"Job {poshJob.JobUID} started.");
            return poshJob;
        }

        public async Task<PSDataCollection<PSObject>> StartScript(PoshJob poshJob) //maybe add withRunspace parameter in the future. I have no idea what a runspace provides.
        {
            PSDataCollection<PSDataCollection<PSObject>> output = new PSDataCollection<PSDataCollection<PSObject>>();
            output.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                RecordOutput(poshJob, sender, e);
            };
            return await poshJob.PoshInstance.InvokeAsync<PSDataCollection<PSObject>, PSDataCollection<PSObject>>(null, output);
        }
    }
}
