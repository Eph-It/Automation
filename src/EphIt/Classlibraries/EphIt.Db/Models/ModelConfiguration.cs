using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
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
    public class AuthenticationConfiguration : IEntityTypeConfiguration<Authentication>
    {
        public void Configure(EntityTypeBuilder<Authentication> builder)
        {
            // Now the table is populated by AuthenticationEnum in project EphIt.Db.Models
            List<Authentication> SeededValues = new List<Authentication>();
            foreach (AuthenticationEnum a in (AuthenticationEnum[])Enum.GetValues(typeof(AuthenticationEnum)))
            {
                SeededValues.Add(new Authentication()
                {
                    AuthenticationId = (short)a,
                    Name = a.ToString()
                });
            }
            builder.HasData(SeededValues.ToArray());
        }
    }
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasOne(d => d.Authentication)
                .WithMany(p => p.Group)
                .HasForeignKey(d => d.AuthenticationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(new Group()
            {
                AuthenticationId = (short)AuthenticationEnum.EphItInternal,
                GroupId = -1
            });
        }
    }
    public class GroupActiveDirectoryConfiguration : IEntityTypeConfiguration<GroupActiveDirectory>
    {
        public void Configure(EntityTypeBuilder<GroupActiveDirectory> builder)
        {
            builder.HasOne(d => d.Group)
                .WithMany(p => p.GroupActiveDirectory)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class GroupAzureActiveDirectoryConfiguration : IEntityTypeConfiguration<GroupAzureActiveDirectory>
    {
        public void Configure(EntityTypeBuilder<GroupAzureActiveDirectory> builder)
        {
            builder.HasOne(d => d.Group)
                .WithMany()
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasOne(d => d.CreatedByUser)
                .WithMany(many => many.Job)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.JobStatus)
                .WithMany(p => p.Job)
                .HasForeignKey(key => key.JobStatusId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.ScriptVersion)
                .WithMany(p => p.Jobs)
                .HasForeignKey(key => key.ScriptVersionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
    public class JobLogConfiguration : IEntityTypeConfiguration<JobLog>
    {
        public void Configure(EntityTypeBuilder<JobLog> builder)
        {
            builder.HasOne(p => p.JobU)
                .WithMany(m => m.JobLog)
                .HasForeignKey(key => key.JobUid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class JobOutputConfiguration : IEntityTypeConfiguration<JobOutput>
    {
        public void Configure(EntityTypeBuilder<JobOutput> builder)
        {
            builder.HasOne(p => p.Job)
                .WithMany(m => m.JobOutput)
                .HasForeignKey(key => key.JobUid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class JobParametersConfiguration : IEntityTypeConfiguration<JobParameters>
    {
        public void Configure(EntityTypeBuilder<JobParameters> builder)
        {
            builder.HasOne(p => p.Job)
                .WithOne(d => d.JobParameters)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class JobQueueConfiguration : IEntityTypeConfiguration<JobQueue>
    {
        public void Configure(EntityTypeBuilder<JobQueue> builder)
        {
            builder.HasOne(p => p.Job)
                .WithOne(d => d.JobQueue)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class JobStatusConfiguration : IEntityTypeConfiguration<JobStatus>
    {
        public void Configure(EntityTypeBuilder<JobStatus> builder)
        {
            // The table is populated by AuthenticationEnum in project EphIt.Db.Models
            List<JobStatus> SeededValues = new List<JobStatus>();
            foreach (JobStatusEnum a in (JobStatusEnum[])Enum.GetValues(typeof(JobStatusEnum)))
            {
                SeededValues.Add(new JobStatus()
                {
                    JobStatusId = (short)a,
                    Status = a.ToString()
                });
            }
            builder.HasData(SeededValues.ToArray());
        }
    }
    public class RbacActionConfiguration : IEntityTypeConfiguration<RbacAction>
    {
        public void Configure(EntityTypeBuilder<RbacAction> builder)
        {
            // The table is populated by RbacActionEnum in project EphIt.Db.Models
            List<RbacAction> SeededValues = new List<RbacAction>();
            foreach (RBACActionEnum a in (RBACActionEnum[])Enum.GetValues(typeof(RBACActionEnum)))
            {
                SeededValues.Add(new RbacAction()
                {
                    RbacActionId = (short)a,
                    Name = a.ToString()
                });
            }
            builder.HasData(SeededValues.ToArray());
        }
    }
    public class RbacObjectConfiguration : IEntityTypeConfiguration<RbacObject>
    {
        public void Configure(EntityTypeBuilder<RbacObject> builder)
        {
            // The table is populated by RbacActionEnum in project EphIt.Db.Models
            List<RbacObject> SeededValues = new List<RbacObject>();
            foreach (RBACObjectEnum a in (RBACObjectEnum[])Enum.GetValues(typeof(RBACObjectEnum)))
            {
                SeededValues.Add(new RbacObject()
                {
                    RbacObjectId = (short)a,
                    Name = a.ToString()
                });
            }
            builder.HasData(SeededValues.ToArray());
        }
    }
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasOne(p => p.CreatedByUser)
                .WithMany(d => d.RoleCreatedByUser)
                .HasForeignKey(key => key.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.ModifiedByUser)
                .WithMany(d => d.RoleModifiedByUser)
                .HasForeignKey(key => key.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
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
    public class ScriptConfiguration : IEntityTypeConfiguration<Script>
    {
        public void Configure(EntityTypeBuilder<Script> builder)
        {
            builder.HasOne(d => d.CreatedByUser)
                .WithMany(p => p.ScriptCreatedByUser)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ModifiedByUser)
                .WithMany(p => p.ScriptModifiedByUser)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class ScriptLanguageConfiguration : IEntityTypeConfiguration<ScriptLanguage>
    {
        public void Configure(EntityTypeBuilder<ScriptLanguage> builder)
        {
            // The table is populated by RbacActionEnum in project EphIt.Db.Models
            List<ScriptLanguage> SeededValues = new List<ScriptLanguage>();
            foreach (ScriptLanguageEnum a in (ScriptLanguageEnum[])Enum.GetValues(typeof(ScriptLanguageEnum)))
            {
                SeededValues.Add(new ScriptLanguage()
                {
                    ScriptLanguageId = (short)a,
                    Language = a.ToString()
                });
            }
            builder.HasData(SeededValues.ToArray());
        }
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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(d => d.Authentication)
                .WithMany(p => p.User)
                .HasForeignKey(d => d.AuthenticationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(new User()
            {
                AuthenticationId = (short)AuthenticationEnum.EphItInternal,
                UserId = -1
            });
        }
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
    public class UserAzureActiveDirectoryConfiguration : IEntityTypeConfiguration<UserAzureActiveDirectory>
    {
        public void Configure(EntityTypeBuilder<UserAzureActiveDirectory> builder)
        {
            builder.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class VariableConfiguration : IEntityTypeConfiguration<Variable>
    {
        public void Configure(EntityTypeBuilder<Variable> builder)
        {
            builder.HasKey(d => d.VariableId);
        }
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
            /*builder.HasOne(p => p.Job)
                .WithMany(many => many.JobObjectIds)
                .HasForeignKey(key => key.JobUid)
                .HasPrincipalKey(pkey => pkey.JobUid);*/
            builder.HasQueryFilter(p => _context.GetUserObjectIds().Contains(p.ObjectId));
        }
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
    public class VRBACScriptToObjectIdConfiguration : IEntityTypeConfiguration<VRBACScriptToObjectId>
    {
        private EphItContext _context;
        public VRBACScriptToObjectIdConfiguration(EphItContext context)
        {
            _context = context;
        }
        public void Configure(EntityTypeBuilder<VRBACScriptToObjectId> builder)
        {
            builder.HasKey("ScriptId", "RoleId", "UserGroupId");
            builder.ToView("v_RBACScriptToObjectId");
            /*builder.HasOne(p => p.Script)
                .WithMany(many => many.ScriptObjectIds)
                .HasForeignKey(key => key.ScriptId)
                .HasPrincipalKey(pkey => pkey.ScriptId);*/
            builder.HasQueryFilter(p => _context.GetUserObjectIds().Contains(p.ObjectId));
        }
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
