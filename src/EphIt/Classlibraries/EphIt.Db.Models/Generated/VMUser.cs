using System;

namespace EphIt.Db.Models
{
    public partial class VMUser
    {
        public VMUser()
        {
            
        }

        public VMUser(User obj)
        {
            UserId = obj.UserId;
            AuthenticationId = obj.AuthenticationId;
        }

        public int UserId { get; set; }
        public short AuthenticationId { get; set; }

    }
}
