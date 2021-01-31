using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class RoleObjectScopeScript
    {
        [Key]
        public int RoleObjectScopeScriptId { get; set; }
        public int ScriptId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Script Script { get; set; }
    }
    
}
