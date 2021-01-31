using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class JobQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid JobUid { get; set; }
        public int ScriptVersionId { get; set; }
        public short ScriptLanguage { get; set; }
        public DateTime Created { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [ForeignKey("JobUid")]
        public virtual Job Job { get; set; }
    }
    
}
