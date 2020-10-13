using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class RoleMembershipUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleMembershipUserId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
    public class RoleMembershipUserConfiguration : IEntityTypeConfiguration<RoleMembershipUser>
    {
        public void Configure(EntityTypeBuilder<RoleMembershipUser> builder)
        {
            builder.HasOne(p => p.Role)
                .WithMany(d => d.RoleMembershipUser)
                .HasForeignKey(key => key.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.User)
                .WithMany(d => d.RoleMembershipUser)
                .HasForeignKey(key => key.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
