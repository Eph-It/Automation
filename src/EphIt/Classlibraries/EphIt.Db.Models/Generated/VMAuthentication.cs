using System;

namespace EphIt.Db.Models
{
    public partial class VMAuthentication
    {
        public VMAuthentication()
        {
            
        }

        public VMAuthentication(Authentication obj)
        {
            AuthenticationId = obj.AuthenticationId;
            Name = obj.Name;
        }

        public short AuthenticationId { get; set; }
        public string Name { get; set; }

    }
}
