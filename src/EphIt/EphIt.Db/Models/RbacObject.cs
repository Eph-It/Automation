using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class RbacObject
    {
        public RbacObject()
        {
            RoleObjectAction = new HashSet<RoleObjectAction>();
        }

        public short RbacObjectId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
    }
}
