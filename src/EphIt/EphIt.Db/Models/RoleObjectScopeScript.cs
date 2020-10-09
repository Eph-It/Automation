using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class RoleObjectScopeScript
    {
        public int RoleObjectScopeScriptId { get; set; }
        public int ScriptId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Script Script { get; set; }
    }
}
