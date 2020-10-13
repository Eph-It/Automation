using System;

namespace EphIt.Db.Models
{
    public partial class VMRoleObjectAction
    {
        public VMRoleObjectAction()
        {
            
        }

        public VMRoleObjectAction(RoleObjectAction obj)
        {
            RoleObjectActionId = obj.RoleObjectActionId;
            RbacActionId = obj.RbacActionId;
            RbacObjectId = obj.RbacObjectId;
            RoleId = obj.RoleId;
        }

        public int RoleObjectActionId { get; set; }
        public short RbacActionId { get; set; }
        public short RbacObjectId { get; set; }
        public int RoleId { get; set; }

    }
}
