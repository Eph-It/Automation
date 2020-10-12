using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EphIt.BL.Audit
{
    public class AuditLogger : IAuditLogger
    {
        public Task AuditLog(AuditObject obj, AuditAction act, AuditStatus status, int? id = null, Guid? uid = null)
        {
            return Task.CompletedTask;
        }
    }
}
