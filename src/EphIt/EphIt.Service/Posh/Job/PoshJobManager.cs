using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using EphIt.Service.Posh.Stream;
using EphIt.Service.Posh.Job;
using EphIt.Service.Posh;
using System.Management.Automation;
using System.Linq;
using Serilog;
using EphIt.BL.JobManager;

namespace EphIt.Service.Posh.Job
{
    public class PoshJobManager : IPoshJobManager
    {
        private ConcurrentQueue<PoshJob> pendingJobQueue = new ConcurrentQueue<PoshJob>();
        private ConcurrentQueue<ProblemJob> problemJobs = new ConcurrentQueue<ProblemJob>();
        private ConcurrentQueue<PoshJob> retryJobs = new ConcurrentQueue<PoshJob>();
        private ConcurrentDictionary<Guid, PoshJob> runningJobs = new ConcurrentDictionary<Guid, PoshJob>();
        private IPoshManager _poshManager;
        //private EphIt.BL.JobManager.IJobManager _jobManager;
        public PoshJobManager(IPoshManager poshManager)
        {
            //_jobManager = jobManager;
            _poshManager = poshManager;
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
                }
                //record any more output
                //done?  record end date, remove from list
                //error?
            }
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
            PoshJob pendingJob;
            pendingJobQueue.TryDequeue(out pendingJob);
            if(pendingJob == null)
            {
                return;
            }
            try
            {
                PoshJob runningJob = _poshManager.RunJob(pendingJob);
                if (runningJob == null)
                {
                    retryJobs.Enqueue(pendingJob);
                }
                else
                {
                    bool success = runningJobs.TryAdd(Guid.NewGuid(), runningJob);
                    if(!success) 
                    {
                        //this is bad and hopefully never happens.
                        Log.Error("Unable to queue the running job, this job will not be monitored.");
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
    }
}
