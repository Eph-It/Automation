using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class UserWindows
    {
        public int UserWindowsId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Sid { get; set; }

        public virtual User User { get; set; }
    }
}
