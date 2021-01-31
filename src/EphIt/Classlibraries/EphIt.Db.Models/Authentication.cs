using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Authentication
    {
        public Authentication()
        {
            User = new HashSet<User>();
            Group = new HashSet<Group>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public short AuthenticationId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Group> Group { get; set; }
    }
    
}
