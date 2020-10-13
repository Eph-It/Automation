using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class ScriptLanguage
    {
        public ScriptLanguage()
        {
            ScriptVersion = new HashSet<ScriptVersion>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ScriptLanguageId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Language { get; set; }

        public virtual ICollection<ScriptVersion> ScriptVersion { get; set; }
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
}
