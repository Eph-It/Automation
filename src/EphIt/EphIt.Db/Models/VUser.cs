using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class VUser
    {
        public int UserId { get; set; }
        public string AuthenticationType { get; set; }
        public short? AuthenticationId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UniqueIdentifier { get; set; }
    }
}
