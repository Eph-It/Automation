using EphIt.Db.Models;
using System;
using System.Threading.Tasks;

namespace EphIt.BL.Audit
{
    public interface IAuditLogger
    {
        Task AuditLog(AuditObject obj, AuditAction act, AuditStatus status, int? id = null, Guid? uid = null);
    }
}
