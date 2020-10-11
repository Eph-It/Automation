using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EphIt.Db.Models
{
    public partial class EphItContext : DbContext
    {
        public EphItContext()
        {
        }

        public EphItContext(DbContextOptions<EphItContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Authentication> Authentication { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobLog> JobLog { get; set; }
        public virtual DbSet<JobStatus> JobStatus { get; set; }
        public virtual DbSet<RbacAction> RbacAction { get; set; }
        public virtual DbSet<RbacObject> RbacObject { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleMembershipUser> RoleMembershipUser { get; set; }
        public virtual DbSet<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual DbSet<RoleObjectScopeJob> RoleObjectScopeJob { get; set; }
        public virtual DbSet<RoleObjectScopeScript> RoleObjectScopeScript { get; set; }
        public virtual DbSet<Script> Script { get; set; }
        public virtual DbSet<ScriptLanguage> ScriptLanguage { get; set; }
        public virtual DbSet<ScriptVersion> ScriptVersion { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserWindows> UserWindows { get; set; }
        public virtual DbSet<VUser> VUser { get; set; }
        public virtual DbSet<VWindowsUser> VWindowsUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=Lab-CM.Home.Lab;Database=EphIt;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.Property(e => e.AuthenticationId)
                    .HasColumnName("Authentication_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.JobUid)
                    .HasName("PK__Job__9EB72944FB035182");

                entity.Property(e => e.JobUid)
                    .HasColumnName("Job_UId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedByUserId).HasColumnName("Created_By_User_Id");

                entity.Property(e => e.Finished).HasColumnType("datetime");

                entity.Property(e => e.JobStatusId).HasColumnName("Job_Status_Id");

                entity.Property(e => e.ScriptId).HasColumnName("Script_Id");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_CreatedBy");

                entity.HasOne(d => d.JobStatus)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.JobStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Job_Status");

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.ScriptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Script");
            });

            modelBuilder.Entity<JobLog>(entity =>
            {
                entity.ToTable("Job_Log");

                entity.Property(e => e.JobLogId).HasColumnName("Job_Log_Id");

                entity.Property(e => e.JobUid).HasColumnName("Job_UId");

                entity.Property(e => e.LogTime)
                    .HasColumnName("Log_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Stream)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.JobU)
                    .WithMany(p => p.JobLog)
                    .HasForeignKey(d => d.JobUid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Log_Job");
            });

            modelBuilder.Entity<JobStatus>(entity =>
            {
                entity.ToTable("Job_Status");

                entity.Property(e => e.JobStatusId)
                    .HasColumnName("Job_Status_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<RbacAction>(entity =>
            {
                entity.ToTable("RBAC_Action");

                entity.Property(e => e.RbacActionId)
                    .HasColumnName("RBAC_Action_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<RbacObject>(entity =>
            {
                entity.ToTable("RBAC_Object");

                entity.Property(e => e.RbacObjectId)
                    .HasColumnName("RBAC_Object_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(15);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedByUserId).HasColumnName("Created_By_User_Id");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("Modified_By_User_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.RoleCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Role_User_CreatedBy");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.RoleModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .HasConstraintName("FK_Role_User_ModifiedBy");
            });

            modelBuilder.Entity<RoleMembershipUser>(entity =>
            {
                entity.ToTable("Role_Membership_User");

                entity.Property(e => e.RoleMembershipUserId)
                    .HasColumnName("Role_Membership_User_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMembershipUser)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Membership_User_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoleMembershipUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Membership_User_User");
            });

            modelBuilder.Entity<RoleObjectAction>(entity =>
            {
                entity.ToTable("Role_Object_Action");

                entity.Property(e => e.RoleObjectActionId).HasColumnName("Role_Object_Action_Id");

                entity.Property(e => e.RbacActionId).HasColumnName("RBAC_Action_Id");

                entity.Property(e => e.RbacObjectId).HasColumnName("RBAC_Object_Id");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.HasOne(d => d.RbacAction)
                    .WithMany(p => p.RoleObjectAction)
                    .HasForeignKey(d => d.RbacActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Object_Action_RBAC_Action");

                entity.HasOne(d => d.RbacObject)
                    .WithMany(p => p.RoleObjectAction)
                    .HasForeignKey(d => d.RbacObjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Object_Action_RBAC_Object");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleObjectAction)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Object_Action_Role");
            });

            modelBuilder.Entity<RoleObjectScopeJob>(entity =>
            {
                entity.ToTable("Role_Object_Scope_Job");

                entity.Property(e => e.RoleObjectScopeJobId).HasColumnName("Role_Object_Scope_Job_Id");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.ScriptId).HasColumnName("Script_Id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleObjectScopeJob)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Object_Scope_Job_Role");

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.RoleObjectScopeJob)
                    .HasForeignKey(d => d.ScriptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Object_Scope_Job_Script");
            });

            modelBuilder.Entity<RoleObjectScopeScript>(entity =>
            {
                entity.ToTable("Role_Object_Scope_Script");

                entity.Property(e => e.RoleObjectScopeScriptId).HasColumnName("Role_Object_Scope_Script_Id");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.ScriptId).HasColumnName("Script_Id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleObjectScopeScript)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Object_Scope_Script_Role");

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.RoleObjectScopeScript)
                    .HasForeignKey(d => d.ScriptId)
                    .HasConstraintName("FK_Role_Object_Scope_Script_Script");
            });

            modelBuilder.Entity<Script>(entity =>
            {
                entity.Property(e => e.ScriptId).HasColumnName("Script_Id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedByUserId).HasColumnName("Created_By_User_Id");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("Modified_By_User_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PublishedVersion).HasColumnName("Published_Version");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ScriptCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Script_User_CreatedBy");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.ScriptModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Script_User_ModifiedBy");
            });

            modelBuilder.Entity<ScriptLanguage>(entity =>
            {
                entity.ToTable("Script_Language");

                entity.Property(e => e.ScriptLanguageId)
                    .HasColumnName("Script_Language_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Version).HasMaxLength(20);
            });

            modelBuilder.Entity<ScriptVersion>(entity =>
            {
                entity.ToTable("Script_Version");

                entity.Property(e => e.ScriptVersionId).HasColumnName("Script_Version_Id");

                entity.Property(e => e.Body).IsRequired();

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedByUserId).HasColumnName("Created_By_User_Id");

                entity.Property(e => e.ScriptId).HasColumnName("Script_Id");

                entity.Property(e => e.ScriptLanguageId).HasColumnName("Script_Language_Id");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ScriptVersion)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Script_Version_User_CreatedBy");

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.ScriptVersion)
                    .HasForeignKey(d => d.ScriptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Script_Version_Script");

                entity.HasOne(d => d.ScriptLanguage)
                    .WithMany(p => p.ScriptVersion)
                    .HasForeignKey(d => d.ScriptLanguageId)
                    .HasConstraintName("FK_Script_Version_Language");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.AuthenticationId).HasColumnName("Authentication_Id");

                entity.HasOne(d => d.Authentication)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.AuthenticationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Authentication");
            });

            modelBuilder.Entity<UserWindows>(entity =>
            {
                entity.ToTable("User_Windows");

                entity.Property(e => e.UserWindowsId).HasColumnName("User_Windows_Id");

                entity.Property(e => e.DisplayName).HasMaxLength(250);

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(64);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Sid)
                    .IsRequired()
                    .HasColumnName("SID")
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserWindows)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Windows_User");
            });

            modelBuilder.Entity<VUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_User");

                entity.Property(e => e.AuthenticationType)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.UniqueIdentifier)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VWindowsUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_Windows_User");

                entity.Property(e => e.DisplayName).HasMaxLength(250);

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(64);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.UniqueIdentifier)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
