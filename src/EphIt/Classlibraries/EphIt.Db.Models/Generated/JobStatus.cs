using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class JobStatus
    {
        public JobStatus()
        {
            Job = new HashSet<Job>();
        }

        public short JobStatusId { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Job> Job { get; set; }
    }
}
