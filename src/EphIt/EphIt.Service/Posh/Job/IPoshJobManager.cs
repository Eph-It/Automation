using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Threading.Tasks;

namespace EphIt.Service.Posh.Job
{
    public interface IPoshJobManager
    {
        public void QueuePendingJob(PoshJob pSJob);
        public PoshJob DequeuePendingJob();
        public bool HasPendingJob();
        public void StartPendingJob();
        public void ProcessRunningJobs();
    }
}
