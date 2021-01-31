namespace EphIt.Db.Models
{
    public class VRBACScriptVersionToObjectId
    {
        public int ScriptId { get; set; }
        public int ScriptVersionId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool UserGroupId { get; set; }
        public string ObjectId { get; set; }
        public ScriptVersion ScriptVersion { get; set; }
    }

}
