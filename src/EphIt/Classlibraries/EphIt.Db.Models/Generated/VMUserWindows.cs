using System;

namespace EphIt.Db.Models
{
    public partial class VMUserWindows
    {
        public VMUserWindows()
        {
            
        }

        public VMUserWindows(UserWindows obj)
        {
            UserWindowsId = obj.UserWindowsId;
            UserId = obj.UserId;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
            UserName = obj.UserName;
            Domain = obj.Domain;
            Email = obj.Email;
            DisplayName = obj.DisplayName;
            Sid = obj.Sid;
        }

        public int UserWindowsId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Sid { get; set; }

    }
}
