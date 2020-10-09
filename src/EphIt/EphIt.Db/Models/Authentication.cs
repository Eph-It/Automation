using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class Authentication
    {
        public Authentication()
        {
            User = new HashSet<User>();
        }

        public short AuthenticationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
