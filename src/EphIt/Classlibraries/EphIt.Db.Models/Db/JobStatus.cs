using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class JobStatus
    {
        public JobStatus()
        {
            Job = new HashSet<Job>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short JobStatusId { get; set; }
        [MaxLength(20)]
        public string Status { get; set; }

        public virtual ICollection<Job> Job { get; set; }
    }
    
}
