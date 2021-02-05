namespace EphIt.Db.Models
{
    public class VRBACScriptToObjectId
    {
        
        public int ScriptId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool UserGroupId { get; set; }
        public string ObjectId { get; set; }
        public Script Script { get; set; }
    }

}
