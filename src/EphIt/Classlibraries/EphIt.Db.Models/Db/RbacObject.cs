using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class RbacObject
    {
        public RbacObject()
        {
            RoleObjectAction = new HashSet<RoleObjectAction>();
            Audit = new HashSet<Audit>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short RbacObjectId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual ICollection<Audit> Audit { get; set; }
    }

    
}
