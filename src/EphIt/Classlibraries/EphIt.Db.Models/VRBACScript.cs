using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class VRBACScript
    {
        [Key]
        public int RowId { get; set; }
        public int RoleId { get; set; }
        public int ScriptId { get; set; }
        public Script Script { get; set; }
    }

}
