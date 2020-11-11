using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;

namespace EphIt.Service.Posh.Job
{
    public interface IPoshJobManager
    {
        public void QueuePendingJob(PoshJob pSJob);
        public PoshJob DequeuePendingJob();
        public bool HasPendingJob();
        public void StartPendingJob();
        public void ProcessRunningJobs();
        public void FaultJob(Guid jobId);
        public void RecordStream(PoshJob poshJob, object sender, DataAddedEventArgs e);
        public void RecordOutput(PoshJob poshJob, object sender, DataAddedEventArgs e);
        public PoshJob ConfigureStreams(PoshJob poshJob);
        public PoshJob RunJob(PoshJob poshJob);

    }
}
