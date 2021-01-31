using System;

namespace EphIt.Db.Models
{
    public class VRBACJobToObjectId
    {
        public int ScriptId { get; set; }
        public int ScriptVersionId { get; set; }
        public Guid JobUid { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool UserGroupId { get; set; }
        public string ObjectId { get; set; }
        public Job Job { get; set; }
    }

}
