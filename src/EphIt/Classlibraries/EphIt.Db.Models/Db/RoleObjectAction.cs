using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class RoleObjectAction
    {
        [Key]
        public int RoleObjectActionId { get; set; }
        public short RbacActionId { get; set; }
        public short RbacObjectId { get; set; }
        public int RoleId { get; set; }

        public virtual RbacAction RbacAction { get; set; }
        public virtual RbacObject RbacObject { get; set; }
        public virtual Role Role { get; set; }
    }
    
}
