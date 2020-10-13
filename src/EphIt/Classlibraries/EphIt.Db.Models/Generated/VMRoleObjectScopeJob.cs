using System;

namespace EphIt.Db.Models
{
    public partial class VMRoleObjectScopeJob
    {
        public VMRoleObjectScopeJob()
        {
            
        }

        public VMRoleObjectScopeJob(RoleObjectScopeJob obj)
        {
            RoleObjectScopeJobId = obj.RoleObjectScopeJobId;
            RoleId = obj.RoleId;
            ScriptId = obj.ScriptId;
        }

        public int RoleObjectScopeJobId { get; set; }
        public int RoleId { get; set; }
        public int ScriptId { get; set; }

    }
}
