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
using Microsoft.EntityFrameworkCore;

namespace EphIt.BL.Authorization
{
    public class EphItAuthRequirement : IAuthorizationRequirement
    {
        public RBACActionEnum RBACAction { get; set; }
        public RBACObjectEnum RBACObject { get; set; }
        public EphItAuthRequirement(RBACActionEnum rbacAction, RBACObjectEnum objectId)
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
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EphItAuthRequirement requirement)
        {
            if (System.Environment.GetEnvironmentVariable("NOAUTH").Equals("True"))
            {
                context.Succeed(requirement);
                return;
            }
            _userAuth.SetAuthenticatedWith(null);
            if (!context.User.Identity.IsAuthenticated)
            {
                Log.Warning("User is not authenticated.");
                context.Fail();
                return;
            }

            var t = _httpContextAccessor.HttpContext.GetRouteData();
            object objectId = null;

            if (t != null)
            {
                foreach (var key in t.Values.Keys)
                {
                    if (key.ToLower().Equals(requirement.RBACObject.ToString().ToLower() + "id"))
                    {
                        objectId = t.Values[key];
                    }
                }
            }

            AuthorizedObjects authObjects = new AuthorizedObjects();

            switch (requirement.RBACObject)
            {
                case RBACObjectEnum.Scripts:
                    if(objectId != null)
                    {
                        authObjects = await _userAuth.GetAuthorizedScripts((int)objectId, requirement.RBACAction);
                    }
                    else
                    {
                        authObjects = await _userAuth.GetAuthorizedScripts(null, requirement.RBACAction);
                    }
                    break;
            }

            if (authObjects.GloballyAuthorized || authObjects.AuthorizedIds.Count > 0)
            {
                // Globally Authorized = all objects so auth passes
                // AuthorizedIds > 0 means user is allowed to perform x action on objectId
                Log.Information($"User {context.User.Identity.Name} is authorized for action {requirement.RBACAction} on object type {requirement.RBACObject}.");
                context.Succeed(requirement);
                return;
            }

            if(objectId != null)
            {
                // This means user tried to do something to an object and the auth check didn't pass
                Log.Warning($"User {context.User.Identity.Name} is NOT authorized for action {requirement.RBACAction} on object type {requirement.RBACObject} with objectId {objectId}");
                context.Fail();
                return;
            }

            // We would reach this point if the user tried to do something with no objectId (ie, create a new script)
            // And they don't have global permissions to do that action or their security role doesn't have any 
            // objects associated with it. So if Role1 has the EditScript permission but no scripts associated with it yet
            // We'd wind up here. Now we simply do a check to see if they are in a role that can do the required action
            // IE if they can do a ScriptEdit, this succeeds because they are trying to create a script.

            var teams = await _userAuth.GetRoleIdsAsync();

            if(await _dbContext.Role.Where(p =>
                    teams.Contains(p.RoleId)
                    && p.RoleObjectAction.Where(obj =>
                        obj.RbacObjectId.Equals((short)requirement.RBACObject) &&
                        obj.RbacActionId.Equals((short)requirement.RBACAction)
                    ).Any()
                )
                .AnyAsync()
                )
            {
                Log.Information($"User {context.User.Identity.Name} is authorized for action {requirement.RBACAction} on object type {requirement.RBACObject}.");
                context.Succeed(requirement);
                return;
            }
            Log.Warning($"User {context.User.Identity.Name} is NOT authorized for action {requirement.RBACAction} on object type {requirement.RBACObject}");
            context.Fail();
            return;
        }
    }
}
