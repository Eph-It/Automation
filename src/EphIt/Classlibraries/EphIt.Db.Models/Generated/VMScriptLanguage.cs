﻿using System;

namespace EphIt.Db.Models
{
    public partial class VMScriptLanguage
    {
        public VMScriptLanguage()
        {
            
        }

        public VMScriptLanguage(ScriptLanguage obj)
        {
            ScriptLanguageId = obj.ScriptLanguageId;
            Language = obj.Language;
        }

        public short ScriptLanguageId { get; set; }
        public string Language { get; set; }

    }
}
