using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class User
    {
        public User()
        {
            Job = new HashSet<Job>();
            RoleCreatedByUser = new HashSet<Role>();
            RoleMembershipUser = new HashSet<RoleMembershipUser>();
            RoleModifiedByUser = new HashSet<Role>();
            ScriptCreatedByUser = new HashSet<Script>();
            ScriptModifiedByUser = new HashSet<Script>();
            ScriptVersion = new HashSet<ScriptVersion>();
            UserActiveDirectory = new HashSet<UserActiveDirectory>();
        }
        [Key]
        public int UserId { get; set; }
        public short AuthenticationId { get; set; }

        public virtual Authentication Authentication { get; set; }
        public virtual ICollection<Job> Job { get; set; }
        public virtual ICollection<Role> RoleCreatedByUser { get; set; }
        public virtual ICollection<RoleMembershipUser> RoleMembershipUser { get; set; }
        public virtual ICollection<Role> RoleModifiedByUser { get; set; }
        public virtual ICollection<Script> ScriptCreatedByUser { get; set; }
        public virtual ICollection<Script> ScriptModifiedByUser { get; set; }
        public virtual ICollection<ScriptVersion> ScriptVersion { get; set; }
        public virtual ICollection<UserActiveDirectory> UserActiveDirectory { get; set; }
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
}
