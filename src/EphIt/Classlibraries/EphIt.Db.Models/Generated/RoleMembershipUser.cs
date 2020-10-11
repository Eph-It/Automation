using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class RoleMembershipUser
    {
        public int RoleMembershipUserId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
