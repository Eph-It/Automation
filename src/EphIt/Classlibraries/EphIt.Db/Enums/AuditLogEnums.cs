using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    
    public enum AuditObject
    {
        Script,
        ScriptVersion,
        Role
    }
    public enum AuditAction
    {
        Edit,
        Delete,
        Modify,
        Execute,
        Create
    }
    public enum AuditStatus
    {
        Success,
        Error,
        Unauthorized
    }
}
