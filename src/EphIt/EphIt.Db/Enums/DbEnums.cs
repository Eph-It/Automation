using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Enums
{
    public enum AuthenticationEnum : short
    {
        Windows = 1,
        EphItInternal = 2
    }
    public enum JobStatusEnum : short
    {
        New = 1,
        Queued = 2,
        InProgress = 3,
        Complete = 10,
        Error = 11,
        Cancelled = 12
    }
    public enum ScriptLanguageEnum : short
    {
        PowerShell5 = 1
    }
    public enum RBACActionsEnum : short
    {
        Create = 1,
        Read = 2,
        Delete = 3,
        Modify = 4,
        Execute = 5
    }

    public enum RBACObjectsId : short
    {
        Scripts = 1,
        Roles = 2,
        Variables = 3,
        Modules = 4,
        Jobs = 5
    }
}
