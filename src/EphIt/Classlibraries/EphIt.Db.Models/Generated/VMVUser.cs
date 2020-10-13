using System;

namespace EphIt.Db.Models
{
    public partial class VMVUser
    {
        public VMVUser()
        {
            
        }

        public VMVUser(VUser obj)
        {
            UserId = obj.UserId;
            AuthenticationType = obj.AuthenticationType;
            AuthenticationId = obj.AuthenticationId;
            DisplayName = obj.DisplayName;
            UserName = obj.UserName;
            Email = obj.Email;
            UniqueIdentifier = obj.UniqueIdentifier;
        }

        public int UserId { get; set; }
        public string AuthenticationType { get; set; }
        public short? AuthenticationId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UniqueIdentifier { get; set; }

    }
}
