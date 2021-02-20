using System;

namespace EphIt.Db.Models
{
    public class VMModule
    {
        public VMModule (Module module)
        {
            ModuleId = module.ModuleId;
            Name = module.Name;
            Data = module.Data;
            Created = module.Created;
            CreatedByUserId = module.CreatedByUserId;
            Modified = module.Modified;
            ModifiedByUserId = module.ModifiedByUserId;
        } 
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedByUserId { get; set; }
    }
}
