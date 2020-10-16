using EphIt.BL.User;
using EphIt.Db.Enums;
using EphIt.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace EphIt.BL.Authorization
{
    public class UserAuthorization : IUserAuthorization
    {
        IEphItUser _userObj;
        EphItContext _db;
        public UserAuthorization(IEphItUser userObj, EphItContext context)
        {
            _userObj = userObj;
            _db = context;
        }
        List<Role> _userRoles;
        List<int> _userRoleIds;
        private int? authenticatedWith = null;
        private IQueryable<RoleMembershipUser> GetRoleQuery()
        {
            return _db.RoleMembershipUser.Where(
                    p => p.UserId.Equals(_userObj.Register().UserId)
                    );
        }
        public async Task<List<Role>> GetRolesAsync()
        {
            if (_userRoles == null)
            {
                _userRoles = await GetRoleQuery()
                    .Include(p => p.Role)
                    .Select(p => p.Role)
                    .ToListAsync();
            }
            return _userRoles;
        }
        public async Task<List<int>> GetRoleIdsAsync()
        {
            if(_userRoles != null)
            {
                return _userRoles.Select(p => p.RoleId).ToList();
            }
            if(_userRoleIds != null)
            {
                return _userRoleIds;
            }
            _userRoleIds = await GetRoleQuery().Select(p => p.RoleId).ToListAsync();
            return _userRoleIds;
        }
        public void SetAuthenticatedWith(int? roleId)
        {
            authenticatedWith = roleId;
        }
        public int? AuthenticatedWith()
        {
            return authenticatedWith;
        }
        public async Task<AuthorizedObjects> GetAuthorizedScripts(int? scriptId = null, RBACActionEnum? action = null)
        {
            var authObjs = new AuthorizedObjects();

            var roleMembershipQuery = GetRoleQuery();
            if(action != null)
            {
                roleMembershipQuery.Where(
                        p=> (
                            p.Role.IsGlobal ||
                            (scriptId.HasValue ? 
                                p.Role.RoleObjectScopeScript.Where(script => script.ScriptId.Equals(scriptId.Value)).Any() : 
                                p.Role.RoleObjectScopeScript.Any())
                            )
                        && p.Role.RoleObjectAction.Where(d => d.RbacActionId == (short)action && d.RbacObjectId == (short)RBACObjectEnum.Scripts).Any()
                    );
            }
            else
            {
                roleMembershipQuery.Where(
                        p => (
                            p.Role.IsGlobal ||
                            (scriptId.HasValue ?
                                p.Role.RoleObjectScopeScript.Where(script => script.ScriptId.Equals(scriptId.Value)).Any() :
                                p.Role.RoleObjectScopeScript.Any())
                            )
                        && p.Role.RoleObjectAction.Where(d => d.RbacObjectId == (short)RBACObjectEnum.Scripts).Any()
                    );
            }
            var roles = await roleMembershipQuery.Include(p => p.Role)
                            .ThenInclude(d => d.RoleObjectScopeScript)
                            .Select(p => p.Role)
                            .ToListAsync();
            
            foreach(var r in roles)
            {
                if (r.IsGlobal)
                {
                    authObjs.GloballyAuthorized = true;
                }
                else
                {
                    foreach(var o in r.RoleObjectScopeScript)
                    {
                        authObjs.AuthorizedIds.Add(new AuthorizedObject()
                        {
                            Id = r.RoleId,
                            Role = o.ScriptId
                        });
                    }
                }
            }

            return authObjs;
        }
    }
    public class AuthorizedObjects
    {
        public AuthorizedObjects()
        {
            AuthorizedIds = new List<AuthorizedObject>();
            GloballyAuthorized = false;
        }
        public bool GloballyAuthorized { get; set; }
        public List<AuthorizedObject> AuthorizedIds { get; set; }
    }
    public class AuthorizedObject
    {
        public int Role { get; set; }
        public int Id { get; set; }
    }
}
