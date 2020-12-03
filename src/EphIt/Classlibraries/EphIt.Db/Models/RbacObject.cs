using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class RbacObject
    {
        public RbacObject()
        {
            RoleObjectAction = new HashSet<RoleObjectAction>();
            Audit = new HashSet<Audit>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short RbacObjectId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual ICollection<Audit> Audit { get; set; }
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
}
