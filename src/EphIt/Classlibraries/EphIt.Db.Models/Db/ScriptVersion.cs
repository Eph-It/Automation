using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class ScriptVersion
    {
        public ScriptVersion()
        {
            Jobs = new HashSet<Job>();
        }
        public int ScriptVersionId { get; set; }
        [Required]
        public string Body { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public short? ScriptLanguageId { get; set; }
        public int ScriptId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual Script Script { get; set; }
        public virtual ScriptLanguage ScriptLanguage { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
    
}
