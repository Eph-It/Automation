using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    public class VRBACJobToObjectId
    {
        public int ScriptId { get; set; }
        public int ScriptVersionId { get; set; }
        public Guid JobUid { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool UserGroupId { get; set; }
        public string ObjectId { get; set; }
        public Job Job { get; set; }
    }
    public class VRBACJobToObjectIdConfiguration : IEntityTypeConfiguration<VRBACJobToObjectId>
    {
        private EphItContext _context;
        public VRBACJobToObjectIdConfiguration(EphItContext context)
        {
            _context = context;
        }
        public void Configure(EntityTypeBuilder<VRBACJobToObjectId> builder)
        {
            builder.HasKey("JobUid", "RoleId", "UserGroupId");
            builder.ToView("v_RBACJobToObjectId");
            builder.HasOne(p => p.Job)
                .WithMany(many => many.JobObjectIds)
                .HasForeignKey(key => key.JobUid)
                .HasPrincipalKey(pkey => pkey.JobUid);
            builder.HasQueryFilter(p => _context.GetUserObjectIds().Contains(p.ObjectId));
        }
    }
}
