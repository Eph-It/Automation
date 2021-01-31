using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    public class VRBACScriptVersionToObjectId
    {
        public int ScriptId { get; set; }
        public int ScriptVersionId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool UserGroupId { get; set; }
        public string ObjectId { get; set; }
        public ScriptVersion ScriptVersion { get; set; }
    }
    public class VRBACScriptVersionToObjectIdConfiguration : IEntityTypeConfiguration<VRBACScriptVersionToObjectId>
    {
        private EphItContext _context;
        public VRBACScriptVersionToObjectIdConfiguration(EphItContext context)
        {
            _context = context;
        }
        public void Configure(EntityTypeBuilder<VRBACScriptVersionToObjectId> builder)
        {
            builder.HasKey("ScriptVersionId", "RoleId", "UserGroupId");
            builder.ToView("v_RBACScriptVersionToObjectId");
            /*builder.HasOne(p => p.ScriptVersion)
                .WithMany(many => many.ScriptVersionObjectIds)
                .HasForeignKey(key => key.ScriptVersionId)
                .HasPrincipalKey(pkey => pkey.ScriptVersionId);*/
            builder.HasQueryFilter(p => _context.GetUserObjectIds().Contains(p.ObjectId));
        }
    }
}
