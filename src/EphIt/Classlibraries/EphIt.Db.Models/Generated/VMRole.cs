using System;

namespace EphIt.Db.Models
{
    public partial class VMRole
    {
        public VMRole()
        {
            
        }

        public VMRole(Role obj)
        {
            RoleId = obj.RoleId;
            Name = obj.Name;
            CreatedByUserId = obj.CreatedByUserId;
            Created = obj.Created;
            ModifiedByUserId = obj.ModifiedByUserId;
            Modified = obj.Modified;
            Description = obj.Description;
            IsGlobal = obj.IsGlobal;
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public string Description { get; set; }
        public bool IsGlobal { get; set; }

    }
}
