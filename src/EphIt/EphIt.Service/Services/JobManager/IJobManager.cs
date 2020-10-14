using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Service.Services.JobManager
{
    public interface IJobManager
    {
        public void QueueJob(PoshJob pSJob);
        public PoshJob DequeueJob();
        public bool HasPendingJob();
        public void StartPoshJob();
        public void ProcessRunningJobs();
    }
}
