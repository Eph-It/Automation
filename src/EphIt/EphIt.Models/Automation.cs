using System;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Models
{
    public class Automation
    {
        public Automation()
        {

        }

        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
    }
}
