using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleMembershipUser = new HashSet<RoleMembershipUser>();
            RoleObjectAction = new HashSet<RoleObjectAction>();
            RoleObjectScopeJob = new HashSet<RoleObjectScopeJob>();
            RoleObjectScopeScript = new HashSet<RoleObjectScopeScript>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public string Description { get; set; }
        public bool IsGlobal { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<RoleMembershipUser> RoleMembershipUser { get; set; }
        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual ICollection<RoleObjectScopeJob> RoleObjectScopeJob { get; set; }
        public virtual ICollection<RoleObjectScopeScript> RoleObjectScopeScript { get; set; }
    }
}
