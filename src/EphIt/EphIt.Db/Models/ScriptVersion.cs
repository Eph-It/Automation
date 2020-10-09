using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class ScriptVersion
    {
        public int ScriptVersionId { get; set; }
        public string Body { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public short? ScriptLanguageId { get; set; }
        public int ScriptId { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual Script Script { get; set; }
        public virtual ScriptLanguage ScriptLanguage { get; set; }
    }
}
