using EphIt.Db.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class RbacAction
    {
        public RbacAction()
        {
            RoleObjectAction = new HashSet<RoleObjectAction>();
            Audit = new HashSet<Audit>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short RbacActionId { get; set; }
        [MaxLength(15)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<RoleObjectAction> RoleObjectAction { get; set; }
        public virtual ICollection<Audit> Audit { get; set; }
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
}
