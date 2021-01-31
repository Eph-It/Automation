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
    
}
