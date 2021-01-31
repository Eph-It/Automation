using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EphIt.Db.Models
{
    public class GroupAzureActiveDirectory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupAzureActiveDirectoryId { get; set; }
        public int GroupId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string GroupName { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenantId { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string ObjectId { get; set; }

        public virtual Group Group { get; set; }
    }
    
}
