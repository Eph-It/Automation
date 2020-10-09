using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class RoleObjectAction
    {
        public int RoleObjectActionId { get; set; }
        public short RbacActionId { get; set; }
        public short RbacObjectId { get; set; }
        public int RoleId { get; set; }

        public virtual RbacAction RbacAction { get; set; }
        public virtual RbacObject RbacObject { get; set; }
        public virtual Role Role { get; set; }
    }
}
