using EphIt.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EphIt.Db.Enums;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EphIt.BL.Authorization
{
    public class EphItAuthRequirement : IAuthorizationRequirement
    {
        public RBACActionsEnum? RBACAction { get; set; }
        public RBACObjectsId RBACObject { get; set; }
        public EphItAuthRequirement(RBACActionsEnum? rbacAction, RBACObjectsId objectId)
        {
            RBACAction = rbacAction;
            RBACObject = objectId;
        }

    }
    public class EphItAuthRequirementHandler : AuthorizationHandler<EphItAuthRequirement>
    {
        private readonly EphItContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAuthorization _userAuth;

        public EphItAuthRequirementHandler(
            EphItContext dbContext
            , IHttpContextAccessor httpContextAccessor
            , IUserAuthorization userAuth
        )
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userAuth = userAuth;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EphItAuthRequirement requirement)
        {
            _userAuth.SetAuthenticatedWith(null);
            if (!context.User.Identity.IsAuthenticated)
            {
                Log.Warning("User is not authenticated.");
                return Task.CompletedTask;
            }
            var teams = _userAuth.GetRoles();
            Log.Information("User found to be on {count} teams", teams.Count);

            if (requirement.RBACAction == null)
            {
                // Action would be null on things like a search endpoint for searching by Name
                var rle = _dbContext.Role.Where(p =>
                        teams.Contains(p.RoleId)
                        && p.RoleObjectAction.Where(obj =>
                            obj.RbacObjectId.Equals((short)requirement.RBACObject)
                        ).Any()
                    )
                    .FirstOrDefault();
                if (rle != null)
                {
                    _userAuth.SetAuthenticatedWith(rle.RoleId);
                    Log.Information("User authenticated with {role}", rle.RoleId);
                    context.Succeed(requirement);
                }
                else
                {
                    Log.Warning("User is not in any roles matching the {requirement}", requirement);
                    context.Fail();
                }
                return Task.CompletedTask;
            }

            var rolesMatching = _dbContext.Role.Where(p =>
                    teams.Contains(p.RoleId)
                    && p.RoleObjectAction.Where(obj =>
                        obj.RbacActionId.Equals((short)requirement.RBACAction)
                        && obj.RbacObjectId.Equals((short)requirement.RBACObject)
                    ).Any()
                )
                .ToList();

            if (rolesMatching.Count == 0)
            {
                Log.Warning("User is not in any roles matching the {requirement}", requirement);
                context.Fail();
                return Task.CompletedTask;
            }
            foreach (var r in rolesMatching)
            {
                if (r.IsGlobal)
                {
                    Log.Information("User authenticated with {roleId}", r.RoleId);
                    _userAuth.SetAuthenticatedWith(r.RoleId);
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            var t = _httpContextAccessor.HttpContext.GetRouteData();
            if (t != null)
            {
                object objectId = null;
                foreach (var key in t.Values.Keys)
                {
                    if (key.ToLower().Equals(requirement.RBACObject.ToString().ToLower() + "id"))
                    {
                        objectId = t.Values[key];
                    }
                }
                if (objectId == null && _httpContextAccessor.HttpContext.Request.Method.ToLower().Equals("post"))
                {
                    Log.Information("User authenticated with {roleId}", rolesMatching[0].RoleId);
                    _userAuth.SetAuthenticatedWith(rolesMatching[0].RoleId);
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                else if (objectId == null)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
                var rolesMatchingIds = rolesMatching.Select(p => p.RoleId).ToList();
                switch (requirement.RBACObject)
                {
                    case RBACObjectsId.Scripts:

                        var foundRole = _dbContext.RoleObjectScopeScript
                                .Where(p =>
                                    p.ScriptId.Equals((int)objectId)
                                    && rolesMatchingIds.Contains(p.RoleId)
                                )
                                .FirstOrDefault();
                        if (foundRole != null)
                        {
                            Log.Information("User authenticated to script with {roleobjectscope}", foundRole.RoleObjectScopeScriptId);
                            context.Succeed(requirement);
                            _userAuth.SetAuthenticatedWith(foundRole.RoleId);
                            return Task.CompletedTask;
                        }
                        break;
                    case RBACObjectsId.Roles:
                        break;
                    case RBACObjectsId.Variables:
                        break;
                    case RBACObjectsId.Modules:
                        break;
                    case RBACObjectsId.Jobs:
                        break;
                    default:
                        break;
                }
            }
            Log.Warning("User not authenticated to {RoleRequirements}", requirement);
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
