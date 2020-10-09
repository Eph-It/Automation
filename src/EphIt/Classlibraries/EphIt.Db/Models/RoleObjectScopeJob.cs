using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class RoleObjectScopeJob
    {
        public int RoleObjectScopeJobId { get; set; }
        public int RoleId { get; set; }
        public int ScriptId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Script Script { get; set; }
    }
}
