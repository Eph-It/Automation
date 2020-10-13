using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class ScriptVersion
    {
        public int ScriptVersionId { get; set; }
        [Required]
        public string Body { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public short? ScriptLanguageId { get; set; }
        public int ScriptId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual Script Script { get; set; }
        public virtual ScriptLanguage ScriptLanguage { get; set; }
    }
    public class ScriptVersionConfiguration : IEntityTypeConfiguration<ScriptVersion>
    {
        public void Configure(EntityTypeBuilder<ScriptVersion> builder)
        {
            builder.HasOne(d => d.CreatedByUser)
                .WithMany(p => p.ScriptVersion)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Script)
                .WithMany(p => p.ScriptVersion)
                .HasForeignKey(d => d.ScriptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.ScriptLanguage)
                .WithMany(p => p.ScriptVersion)
                .HasForeignKey(d => d.ScriptLanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
