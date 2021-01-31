using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class UserAzureActiveDirectory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAzureActiveDirectoryId { get; set; }
        public int UserId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenantId { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string ObjectId { get; set; }

        public virtual User User { get; set; }
    }
    
}
