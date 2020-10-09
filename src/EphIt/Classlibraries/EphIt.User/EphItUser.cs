using EphIt.Db.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;

namespace EphIt.User
{
    public class EphItUser : IEphItUser
    {
        private VUser _user;
        private EphItContext _db;
        private IHttpContextAccessor _httpContext;
        public EphItUser(EphItContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContext = httpContextAccessor;
        }
        private VUser GetUser(string authType, string uniqueIdentifier)
        {
            _user = _db.VUser.Where
                    (
                        p =>
                            p.AuthenticationType.Equals(authType)
                            && p.UniqueIdentifier.Equals(uniqueIdentifier)
                    )
                    .FirstOrDefault();
            return _user;
        }
        private EphIt.Db.Models.User NewUser(short authType)
        {
            var newUser = new EphIt.Db.Models.User();
            newUser.AuthenticationId = authType;
            _db.Add(newUser);
            _db.SaveChanges();
            return newUser;
        }
        private VUser RegisterWindows(WindowsIdentity user)
        {
            var authType = "Windows";
            string uniqueId = user.User.ToString();

            var ephUser = GetUser(authType, uniqueId);
            if(ephUser != null) { return ephUser; }

            var newUser = NewUser(1);

            var windowsPrincipal = new WindowsPrincipal(user);
            var splitUser = user.Name.Split('\\');
            
            var newWindowsUser = new EphIt.Db.Models.UserWindows();
            newWindowsUser.UserId = newUser.UserId;
            newWindowsUser.Sid = uniqueId;
            newWindowsUser.UserName = user.Name.Split('\\').Last();
            newWindowsUser.Domain = splitUser.Count() > 1 ? splitUser[0] : "WORKGROUP";
            _db.Add(newWindowsUser);
            _db.SaveChanges();

            return GetUser(authType, uniqueId);
        }
        public VUser RegisterCurrent()
        {
            return RegisterWindows(WindowsIdentity.GetCurrent());
        }
        public VUser Register()
        {
            if (!_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new AuthenticationException("User not authenticated");
            }

            if(_user != null) { return _user; }

            var userId = _httpContext.HttpContext.User.Identity;
            if(userId is WindowsIdentity)
            {
                _user = RegisterWindows((WindowsIdentity)userId);
            }
            return _user;
        }
        private ICollection<string> GetWindowsGroups()
        {
            var userId = (WindowsIdentity)_httpContext.HttpContext.User.Identity;
            var groupMembership = new HashSet<string>();
            foreach(var g in userId.Groups)
            {
                groupMembership.Add(g.Value);
            }
            return groupMembership;
        }
        public ICollection<string> GetGroupIds()
        {
            if (_httpContext.HttpContext.User.Identity is WindowsIdentity)
            {
                return GetWindowsGroups();
            }
            return null;
        }
    }
}
