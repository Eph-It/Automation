using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Authentication { get; set; }

        public AuthenticationType AuthenticationType { get; set; }
    }
}
