using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EphIt.Db.Models
{
    public class Audit
    {
        [Key]
        public long Audit_Id { get; set; }

        public short RbacActionId { get; set; }
        public short RbacObjectId { get; set; }

        public DateTime Created { get; set; }

        public int UserId { get; set; }

        public virtual RbacAction RbacAction { get; set; }
        public virtual RbacObject RbacObject { get; set; }

        public virtual User User { get; set; }
    }
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.Property(p => p.Audit_Id).ValueGeneratedOnAdd();

            builder.HasOne(d => d.RbacAction)
                .WithMany(p => p.Audit)
                .HasForeignKey(d => d.RbacActionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.RbacObject)
                .WithMany(p => p.Audit)
                .HasForeignKey(d => d.RbacObjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(d => d.Audit)
                .HasForeignKey(key => key.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
