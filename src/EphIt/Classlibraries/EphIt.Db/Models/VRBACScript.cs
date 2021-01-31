using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EphIt.Db.Models
{
    public class VRBACScript
    {
        [Key]
        public int RowId { get; set; }
        public int RoleId { get; set; }
        public int ScriptId { get; set; }
        public Script Script { get; set; }
    }
    public class VRBACScriptConfiguration : IEntityTypeConfiguration<VRBACScript>
    {
        public void Configure(EntityTypeBuilder<VRBACScript> builder)
        {
            builder.ToView("v_RBACScript");
            /*builder.HasOne(p => p.Script)
                .WithMany(many => many.ScriptRoles)
                .HasForeignKey(key => key.ScriptId)
                .HasPrincipalKey(pkey => pkey.ScriptId);*/
            builder.HasQueryFilter(filter => filter.RoleId == 555);
        }
    }
}
