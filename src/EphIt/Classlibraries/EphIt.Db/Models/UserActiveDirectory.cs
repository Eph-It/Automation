using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class UserActiveDirectory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserActiveDirectoryId { get; set; }
        public int UserId { get; set; }
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Domain { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(250)]
        public string DisplayName { get; set; }
        [Required]
        [MaxLength(100)]
        public string SID { get; set; }

        public virtual User User { get; set; }
    }
    public class UserActiveDirectoryConfiguration : IEntityTypeConfiguration<UserActiveDirectory>
    {
        public void Configure(EntityTypeBuilder<UserActiveDirectory> builder)
        {
            builder.HasOne(d => d.User)
                .WithMany(p => p.UserActiveDirectory)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
