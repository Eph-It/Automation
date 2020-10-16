using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Threading.Tasks;

namespace EphIt.Service.Posh.Job
{
    public interface IJobManager
    {
        public void QueueJob(PoshJob pSJob);
        public PoshJob DequeueJob();
        public bool HasPendingJob();
        public void StartPendingJob();
        public void ProcessRunningJobs();
    }
}
