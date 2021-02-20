using System;

namespace EphIt.Db.Models
{
    public partial class VMScript
    {
        public VMScript()
        {
            
        }

        public VMScript(Script obj)
        {
            ScriptId = obj.ScriptId;
            Created = obj.Created;
            CreatedByUserId = obj.CreatedByUserId;
            Modified = obj.Modified;
            ModifiedByUserId = obj.ModifiedByUserId;
            PublishedVersion = obj.PublishedVersion;
            Name = obj.Name;
            Description = obj.Description;
            IsDeleted = obj.IsDeleted;
        }

        public int ScriptId { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedByUserId { get; set; }
        public int? PublishedVersion { get; set; }
        public int? NewestVersion {get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

    }
}
