using EphIt.BL.Audit;
using EphIt.BL.Authorization;
using EphIt.BL.User;
using EphIt.Db.Enums;
using EphIt.Db.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EphIt.BL.Script
{
    public class ScriptManager : IScriptManager
    {
        private IEphItUser _ephItUser;
        private EphItContext _dbContext;
        private IUserAuthorization _userAuth;
        private IAuditLogger _auditLogger;
        public ScriptManager(EphItContext dbContext, IEphItUser ephItUser, IUserAuthorization userAuth, IAuditLogger auditLogger)
        {
            _dbContext = dbContext;
            _ephItUser = ephItUser;
            _userAuth = userAuth;
            _auditLogger = auditLogger;
        }

        public async Task<Db.Models.Script> FindAsync(int scriptId, bool includePublished = false)
        {
            var query = _dbContext.Script.Where(p => p.ScriptId == scriptId);
            if (includePublished)
            {
                query.Include(p => p.PublishedVersion);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Db.Models.Script>> SafeSearchScriptsAsync(string name, bool includePublished = false)
        {
            IQueryable<Db.Models.Script> scriptQuery;

            var authorizedScripts = await _userAuth.GetAuthorizedScripts(null, Db.Enums.RBACActionsEnum.Read);
            if (authorizedScripts.GloballyAuthorized)
            {
                scriptQuery = _dbContext.Script.Where(p => p.Name.Contains(name));
            }
            else
            {
                var authorizedScriptIds = authorizedScripts.AuthorizedIds.Select(p => p.Id).ToList();
                scriptQuery = _dbContext.Script.Where(p => p.Name.Contains(name) && authorizedScriptIds.Contains(p.ScriptId));
            }
            
            if (includePublished)
            {
                scriptQuery.Include(p => p.PublishedVersion);
            }
            return await scriptQuery.ToListAsync();
        }

        public async Task<int> NewAsync(string name, string description)
        {
            var userId = _ephItUser.Register().UserId;

            // This will return the new scriptId and then the Blazor site will
            // Immediately redirect from the "new" page to the "Details" page of this
            // Script - so we want to ensure they have permission to it.
            var userRoleId = await _dbContext.RoleObjectAction
                                .Where(p => 
                                    p.RbacActionId.Equals((short)RBACActionsEnum.Create) 
                                    && p.RbacObjectId.Equals((short)RBACObjectsId.Scripts)
                                    && p.Role.RoleMembershipUser.Where(us => us.UserId.Equals(userId)).Any()
                                )
                                .Select(p => p.RoleId)
                                .FirstAsync();

            var newScript = new EphIt.Db.Models.Script
            {
                Created = DateTime.UtcNow,
                CreatedByUserId = userId,
                Description = description,
                Name = name,
                Modified = DateTime.UtcNow,
                ModifiedByUserId = userId
            };
            _dbContext.Add(newScript);
            await _dbContext.SaveChangesAsync();

            var roleAsyncTask = AssociateWithRoleAsync(newScript.ScriptId, userRoleId);
            var auditTask = _auditLogger.AuditLog(AuditObject.Script, AuditAction.Create, AuditStatus.Success, newScript.ScriptId);

            await Task.WhenAll(roleAsyncTask, auditTask);

            return newScript.ScriptId;
        }

        public async Task AssociateWithRoleAsync(int scriptId, int roleId)
        {
            if(!await _dbContext.RoleObjectAction
                    .Where(p => p.RbacObjectId.Equals((short)RBACObjectsId.Scripts) && p.RoleId.Equals(roleId))
                    .AnyAsync()
            )
            {
                throw new InvalidOperationException($"Provided RoleId {roleId} does not have access to scripts");
            }
            if(await _dbContext.Role.Where(p => p.RoleId.Equals(roleId)).Select(p => p.IsGlobal).FirstAsync())
            {
                Log.Information($"Provided role {roleId} is a global role so no reason to manually associate script to it");
                return;
            }
            if(await _dbContext.RoleObjectScopeScript
                    .Where(p => p.RoleId.Equals(roleId) && p.ScriptId.Equals(scriptId))
                    .AnyAsync()
            )
            {
                Log.Information($"Provided role {roleId} already has {scriptId} associated to it");
                return;
            }

            var newRoleObjScope = new RoleObjectScopeScript()
            {
                RoleId = roleId,
                ScriptId = scriptId
            };
            _dbContext.Add(newRoleObjScope);
            await _dbContext.SaveChangesAsync();
            await _auditLogger.AuditLog(AuditObject.Role, AuditAction.Modify, AuditStatus.Success, newRoleObjScope.RoleId);
        }
    
        public async Task Update(int scriptId, string name = null, string description = null, int? PublishedVersion = null)
        {
            var script = await _dbContext.Script.FindAsync(scriptId);
            bool updatedScript = false;

            if(description != null)
            {
                script.Description = description;
                updatedScript = true;
            }
            if (!string.IsNullOrEmpty(name))
            {
                script.Name = name;
                updatedScript = true;
            }
            if (PublishedVersion.HasValue)
            {
                script.PublishedVersion = PublishedVersion;
                updatedScript = true;
            }
            if (updatedScript)
            {
                script.ModifiedByUserId = _ephItUser.Register().UserId;
                script.Modified = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                await _auditLogger.AuditLog(AuditObject.Script, AuditAction.Modify, AuditStatus.Success, scriptId);
            }
        }
        public async Task Delete(int scriptId)
        {
            var script = await _dbContext.Script.FindAsync(scriptId);
            script.IsDeleted = true;
            script.ModifiedByUserId = _ephItUser.Register().UserId;
            script.Modified = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            await _auditLogger.AuditLog(AuditObject.Script, AuditAction.Delete, AuditStatus.Success, scriptId);
        }
    }
}
