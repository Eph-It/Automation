using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Role
    {
        public Role()
        {
            RoleMembershipUser = new HashSet<RoleMembershipUser>();
            RoleObjectAction = new HashSet<RoleObjectAction>();
            RoleObjectScopeScript = new HashSet<RoleObjectScopeScript>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime Modified { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public bool IsGlobal { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<RoleMembershipUser> RoleMembershipUser { get; set; }
        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual ICollection<RoleObjectScopeScript> RoleObjectScopeScript { get; set; }
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
}
