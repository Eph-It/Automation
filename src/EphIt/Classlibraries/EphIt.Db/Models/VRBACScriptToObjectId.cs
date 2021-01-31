using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    public class VRBACScriptToObjectId
    {
        
        public int ScriptId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool UserGroupId { get; set; }
        public string ObjectId { get; set; }
        public Script Script { get; set; }
    }
    public class VRBACScriptToObjectIdConfiguration : IEntityTypeConfiguration<VRBACScriptToObjectId>
    {
        private EphItContext _context;
        public VRBACScriptToObjectIdConfiguration(EphItContext context)
        {
            _context = context;
        }
        public void Configure(EntityTypeBuilder<VRBACScriptToObjectId> builder)
        {
            builder.HasKey("ScriptId","RoleId","UserGroupId");
            builder.ToView("v_RBACScriptToObjectId");
            /*builder.HasOne(p => p.Script)
                .WithMany(many => many.ScriptObjectIds)
                .HasForeignKey(key => key.ScriptId)
                .HasPrincipalKey(pkey => pkey.ScriptId);*/
            builder.HasQueryFilter(p => _context.GetUserObjectIds().Contains(p.ObjectId));
        }
    }
}
