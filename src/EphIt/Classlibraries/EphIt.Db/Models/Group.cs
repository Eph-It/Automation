using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Group
    {
        public Group()
        {
            GroupActiveDirectory = new HashSet<GroupActiveDirectory>();
        }
        [Key]
        public int GroupId { get; set; }
        public short AuthenticationId { get; set; }

        public virtual Authentication Authentication { get; set; }

        public virtual ICollection<GroupActiveDirectory> GroupActiveDirectory { get; set; }
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
}
