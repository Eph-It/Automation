using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class User
    {
        public User()
        {
            Job = new HashSet<Job>();
            RoleCreatedByUser = new HashSet<Role>();
            RoleMembershipUser = new HashSet<RoleMembershipUser>();
            RoleModifiedByUser = new HashSet<Role>();
            ScriptCreatedByUser = new HashSet<Script>();
            ScriptModifiedByUser = new HashSet<Script>();
            ScriptVersion = new HashSet<ScriptVersion>();
            UserWindows = new HashSet<UserWindows>();
        }

        public int UserId { get; set; }
        public short AuthenticationId { get; set; }

        public virtual Authentication Authentication { get; set; }
        public virtual ICollection<Job> Job { get; set; }
        public virtual ICollection<Role> RoleCreatedByUser { get; set; }
        public virtual ICollection<RoleMembershipUser> RoleMembershipUser { get; set; }
        public virtual ICollection<Role> RoleModifiedByUser { get; set; }
        public virtual ICollection<Script> ScriptCreatedByUser { get; set; }
        public virtual ICollection<Script> ScriptModifiedByUser { get; set; }
        public virtual ICollection<ScriptVersion> ScriptVersion { get; set; }
        public virtual ICollection<UserWindows> UserWindows { get; set; }
    }
}
