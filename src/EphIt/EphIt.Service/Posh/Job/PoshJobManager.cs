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
        private ConcurrentQueue<PoshJob> jobQueue = new ConcurrentQueue<PoshJob>();
        private ConcurrentQueue<ProblemJob> problemJobs = new ConcurrentQueue<ProblemJob>();
        private ConcurrentQueue<PoshJob> retryJobs = new ConcurrentQueue<PoshJob>();
        private ConcurrentQueue<Task<PSDataCollection<PSObject>>> runningJobs = new ConcurrentQueue<Task<PSDataCollection<PSObject>>>();
        private IPoshManager _poshManager;
        private IStreamHelper _streamHelper;
        private IJobManager _jobManager;
        public PoshJobManager(IPoshManager runspaceManager, IStreamHelper streamHelper, IJobManager jobManager)
        {
            _jobManager = jobManager;
            _poshManager = runspaceManager;
            _streamHelper = streamHelper;
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
            jobQueue.Enqueue(job);
        }
        public PoshJob DequeueJob()
        {
            PoshJob result;
            jobQueue.TryDequeue(out result);
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public bool HasPendingJob()
        {
            return !jobQueue.IsEmpty;
        }
        public void StartPendingJob()
        {   
            
            if(!HasPendingJob())
            {
                return;
            }
            if(!_poshManager.RunspaceAvailable())
            {
                return;
            }
            PoshJob pendingJob;
            jobQueue.TryDequeue(out pendingJob);
            if(pendingJob == null)
            {
                return;
            }
            try
            {
                var runningJob = _poshManager.RunJob(pendingJob);
                if (runningJob == null)
                {
                    retryJobs.Enqueue(pendingJob);
                }
                else
                {
                    runningJobs.Enqueue(runningJob);
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
