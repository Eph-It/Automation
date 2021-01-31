using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class JobParameters
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid JobUid { get; set; }
        public string Parameters { get; set; }

        [ForeignKey("JobUid")]
        public virtual Job Job { get; set; }
    }
    
}
