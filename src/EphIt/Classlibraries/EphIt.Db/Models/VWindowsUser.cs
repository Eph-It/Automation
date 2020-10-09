using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class VWindowsUser
    {
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
