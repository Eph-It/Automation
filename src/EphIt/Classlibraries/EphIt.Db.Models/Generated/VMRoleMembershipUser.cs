using System;

namespace EphIt.Db.Models
{
    public partial class VMRoleMembershipUser
    {
        public VMRoleMembershipUser()
        {
            
        }

        public VMRoleMembershipUser(RoleMembershipUser obj)
        {
            RoleMembershipUserId = obj.RoleMembershipUserId;
            RoleId = obj.RoleId;
            UserId = obj.UserId;
        }

        public int RoleMembershipUserId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

    }
}
