using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EphIt.Models
{
    public class AuthenticationType
    {
        public AuthenticationType()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Type { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
