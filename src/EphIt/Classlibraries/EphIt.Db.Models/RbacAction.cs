using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class RbacAction
    {
        public RbacAction()
        {
            RoleObjectAction = new HashSet<RoleObjectAction>();
            Audit = new HashSet<Audit>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short RbacActionId { get; set; }
        [MaxLength(15)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual ICollection<Audit> Audit { get; set; }
    }
    
}
