using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class RoleObjectAction
    {
        [Key]
        public int RoleObjectActionId { get; set; }
        public short RbacActionId { get; set; }
        public short RbacObjectId { get; set; }
        public int RoleId { get; set; }

        public virtual RbacAction RbacAction { get; set; }
        public virtual RbacObject RbacObject { get; set; }
        public virtual Role Role { get; set; }
    }
    public class RoleObjectActionConfiguration : IEntityTypeConfiguration<RoleObjectAction>
    {
        public void Configure(EntityTypeBuilder<RoleObjectAction> builder)
        {
            builder.HasOne(d => d.RbacAction)
                .WithMany(p => p.RoleObjectAction)
                .HasForeignKey(d => d.RbacActionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.RbacObject)
                .WithMany(p => p.RoleObjectAction)
                .HasForeignKey(d => d.RbacObjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Role)
                .WithMany(p => p.RoleObjectAction)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
