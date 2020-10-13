using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EphIt.Db.Models
{
    public class RoleObjectScopeScript
    {
        [Key]
        public int RoleObjectScopeScriptId { get; set; }
        public int ScriptId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Script Script { get; set; }
    }
    public class RoleObjectScopeScriptConfiguration : IEntityTypeConfiguration<RoleObjectScopeScript>
    {
        public void Configure(EntityTypeBuilder<RoleObjectScopeScript> builder)
        {
            builder.HasOne(d => d.Role)
                .WithMany(p => p.RoleObjectScopeScript)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Script)
                .WithMany(p => p.RoleObjectScopeScript)
                .HasForeignKey(d => d.ScriptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
