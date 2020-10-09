using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class ScriptLanguage
    {
        public ScriptLanguage()
        {
            ScriptVersion = new HashSet<ScriptVersion>();
        }

        public short ScriptLanguageId { get; set; }
        public string Language { get; set; }
        public string Version { get; set; }

        public virtual ICollection<ScriptVersion> ScriptVersion { get; set; }
    }
}
