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
            ScriptVersionId = obj.ScriptVersionId;
            JobStatusId = obj.JobStatusId;
            CreatedByUserId = obj.CreatedByUserId;
            Created = obj.Created;
            Finish = obj.Finish;
        }

        public Guid JobUid { get; set; }
        public int ScriptVersionId { get; set; }
        public short JobStatusId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finish { get; set; }

    }
}
