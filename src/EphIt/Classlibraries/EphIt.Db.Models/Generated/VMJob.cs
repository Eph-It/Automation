using System;

namespace EphIt.Db.Models
{
    public partial class VMJob
    {
        public VMJob()
        {
            
        }

        public VMJob(Job obj)
        {
            JobUid = obj.JobUid;
            ScriptId = obj.ScriptId;
            JobStatusId = obj.JobStatusId;
            CreatedByUserId = obj.CreatedByUserId;
            Created = obj.Created;
            Finished = obj.Finished;
        }

        public Guid JobUid { get; set; }
        public int ScriptId { get; set; }
        public short JobStatusId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }

    }
}
