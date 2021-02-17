using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedByUserId { get; set; }
    }
}
