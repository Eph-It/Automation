using System;

namespace EphIt.Db.Models
{
    public partial class VMRbacAction
    {
        public VMRbacAction()
        {
            
        }

        public VMRbacAction(RbacAction obj)
        {
            RbacActionId = obj.RbacActionId;
            Name = obj.Name;
        }

        public short RbacActionId { get; set; }
        public string Name { get; set; }

    }
}
