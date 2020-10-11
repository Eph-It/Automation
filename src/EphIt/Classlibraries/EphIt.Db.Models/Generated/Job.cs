using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class Job
    {
        public Job()
        {
            JobLog = new HashSet<JobLog>();
        }

        public Guid JobUid { get; set; }
        public int ScriptId { get; set; }
        public short JobStatusId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual JobStatus JobStatus { get; set; }
        public virtual Script Script { get; set; }
        public virtual ICollection<JobLog> JobLog { get; set; }
    }
}
