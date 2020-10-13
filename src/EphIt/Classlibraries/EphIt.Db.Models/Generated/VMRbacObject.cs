using System;

namespace EphIt.Db.Models
{
    public partial class VMRbacObject
    {
        public VMRbacObject()
        {
            
        }

        public VMRbacObject(RbacObject obj)
        {
            RbacObjectId = obj.RbacObjectId;
            Name = obj.Name;
        }

        public short RbacObjectId { get; set; }
        public string Name { get; set; }

    }
}
