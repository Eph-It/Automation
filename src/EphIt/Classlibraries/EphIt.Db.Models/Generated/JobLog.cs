using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class JobLog
    {
        public long JobLogId { get; set; }
        public Guid JobUid { get; set; }
        public string Message { get; set; }
        public string Stream { get; set; }
        public DateTime LogTime { get; set; }

        public virtual Job JobU { get; set; }
    }
}
