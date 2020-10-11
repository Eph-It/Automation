using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class RbacAction
    {
        public RbacAction()
        {
            RoleObjectAction = new HashSet<RoleObjectAction>();
        }

        public short RbacActionId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
    }
}
