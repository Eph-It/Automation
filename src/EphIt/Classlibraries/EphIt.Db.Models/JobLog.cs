using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public partial class JobLog
    {
        [Key]
        public Guid JobLogId { get; set; }
        public Guid JobUid { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public short LevelId { get; set; }
        public DateTime LogTime { get; set; }

        public virtual Job JobU { get; set; }
    }
    
}
