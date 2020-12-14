using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Authentication
    {
        public Authentication()
        {
            User = new HashSet<User>();
            Group = new HashSet<Group>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public short AuthenticationId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Group> Group { get; set; }
    }
    public class AuthenticationConfiguration : IEntityTypeConfiguration<Authentication>
    {
        public void Configure(EntityTypeBuilder<Authentication> builder)
        {
            // Now the table is populated by AuthenticationEnum in project EphIt.Db.Models
            List<Authentication> SeededValues = new List<Authentication>();
            foreach(AuthenticationEnum a in (AuthenticationEnum[])Enum.GetValues(typeof(AuthenticationEnum)))
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
}
