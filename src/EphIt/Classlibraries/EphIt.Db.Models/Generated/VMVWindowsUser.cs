using System;

namespace EphIt.Db.Models
{
    public partial class VMVWindowsUser
    {
        public VMVWindowsUser()
        {
            
        }

        public VMVWindowsUser(VWindowsUser obj)
        {
            UserId = obj.UserId;
            UserName = obj.UserName;
            DisplayName = obj.DisplayName;
            Email = obj.Email;
            UniqueIdentifier = obj.UniqueIdentifier;
            Domain = obj.Domain;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UniqueIdentifier { get; set; }
        public string Domain { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
