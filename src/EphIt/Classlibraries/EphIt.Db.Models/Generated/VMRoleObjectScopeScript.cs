using System;

namespace EphIt.Db.Models
{
    public partial class VMRoleObjectScopeScript
    {
        public VMRoleObjectScopeScript()
        {
            
        }

        public VMRoleObjectScopeScript(RoleObjectScopeScript obj)
        {
            RoleObjectScopeScriptId = obj.RoleObjectScopeScriptId;
            ScriptId = obj.ScriptId;
            RoleId = obj.RoleId;
        }

        public int RoleObjectScopeScriptId { get; set; }
        public int ScriptId { get; set; }
        public int RoleId { get; set; }

    }
}
