using System;

namespace EphIt.Db.Models
{
    public partial class VMJobStatus
    {
        public VMJobStatus()
        {
            
        }

        public VMJobStatus(JobStatus obj)
        {
            JobStatusId = obj.JobStatusId;
            Status = obj.Status;
        }

        public short JobStatusId { get; set; }
        public string Status { get; set; }

    }
}
