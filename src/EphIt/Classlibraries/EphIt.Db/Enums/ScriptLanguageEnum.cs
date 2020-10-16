using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    [Flags]
    public enum ScriptLanguageEnum : short
    {
        PowerShell = 1,
        PowerShellCore = 2
    }
    /*
     * When using flags - available numbers for the enum are:
     * 1
     * 2
     * 4
     * 8
     * 16
     * 32
     * 64
     * 128
     * 256
     * etc.
     * max short value is 32,000
     */ 
}
