using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public partial class JobOutput
    {
        [Key]
        public Guid JobOutputId { get; set; }
        public Guid JobUid { get; set; }
        public string Type { get; set; }
        public string JsonValue { get; set; }
        public byte[] ByteArrayValue { get; set; }
        public DateTime OutputTime { get; set; }

        public virtual Job Job { get; set; }
    }
    
}
