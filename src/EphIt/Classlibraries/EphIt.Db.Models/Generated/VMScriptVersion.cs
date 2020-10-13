using System;

namespace EphIt.Db.Models
{
    public partial class VMScriptVersion
    {
        public VMScriptVersion()
        {
            
        }

        public VMScriptVersion(ScriptVersion obj)
        {
            ScriptVersionId = obj.ScriptVersionId;
            Body = obj.Body;
            Version = obj.Version;
            Created = obj.Created;
            CreatedByUserId = obj.CreatedByUserId;
            ScriptLanguageId = obj.ScriptLanguageId;
            ScriptId = obj.ScriptId;
            IsDeleted = obj.IsDeleted;
        }

        public int ScriptVersionId { get; set; }
        public string Body { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public short? ScriptLanguageId { get; set; }
        public int ScriptId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
