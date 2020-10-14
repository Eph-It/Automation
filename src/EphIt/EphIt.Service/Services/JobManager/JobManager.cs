using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using EphIt.Service.Services.Agent;
using EphIt.Service.Services.JobManager;
using EphIt.Service.Services.RunspaceManager;
using System.Management.Automation;

namespace EphIt.Service.Services.JobManager
{
    public class JobManager : IJobManager
    {
        private ConcurrentQueue<PoshJob> _jobQueue = new ConcurrentQueue<PoshJob>();
        private List<Task<PSDataCollection<PSObject>>> runningJobs = new List<Task<PSDataCollection<PSObject>>>();
        private IRunspaceManager _runspaceManager;

        public JobManager(IRunspaceManager runspaceManager)
        {
            _runspaceManager = runspaceManager;
        }
        public void ProcessRunningJobs()
        {
            foreach(var job in runningJobs)
            {
                //record any more output
                //done?  record end date, remove from list
                //error?
                
            }
        }
        public void QueueJob(PoshJob job)
        {
            _jobQueue.Enqueue(job);
        }
        public PoshJob DequeueJob()
        {
            PoshJob result;
            _jobQueue.TryDequeue(out result);
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public bool HasPendingJob()
        {
            if(_jobQueue.IsEmpty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void StartPoshJob()
        {
            if(!HasPendingJob())
            {
                return;
            }
            Runspace rs = _runspaceManager.GetRunspace();
            if(rs == null)
            {
                return; //no runspaces available
            }
            PoshJob pendingJob;
            _jobQueue.TryDequeue(out pendingJob);
            if(pendingJob != null)
            {
                runningJobs.Add(Job(pendingJob, rs));
            }
        }
        public Task<PSDataCollection<PSObject>> Job(PoshJob poshJob, Runspace runspace)
        {
            PowerShell ps = PowerShell.Create();
            ps.Runspace = runspace;
            //capture stream output
            ps.Streams.Verbose.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                LogVerbose(poshJob, sender, e);
            };
            return ps.InvokeAsync<PSDataCollection<PSObject>>(poshJob.Script);
        }
        private void LogVerbose(PoshJob poshJob, object sender, DataAddedEventArgs e) { 
            //record this stream 
        }
    }
}
