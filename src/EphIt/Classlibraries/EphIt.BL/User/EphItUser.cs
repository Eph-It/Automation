using EphIt.Db.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;

namespace EphIt.BL.User
{
    public class EphItUser : IEphItUser
    {
        private Db.Models.User _user;
        private EphItContext _db;
        private IHttpContextAccessor _httpContext;
        public EphItUser(EphItContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContext = httpContextAccessor;
        }
        private Db.Models.User GetUser(string authType, string uniqueIdentifier)
        {
            switch (authType)
            {
                case "ActiveDirectory":
                    return _db.UserActiveDirectory
                            .Where(p => p.SID.Equals(uniqueIdentifier))
                            .Select(p => p.User)
                            .FirstOrDefault();
                default:
                    break;
            }
            return null;
        }
        private Db.Models.User NewUser(short authType)
        {
            var newUser = new Db.Models.User();
            newUser.AuthenticationId = authType;
            _db.Add(newUser);
            _db.SaveChanges();
            return newUser;
        }
        private Db.Models.User RegisterActiveDirectory(WindowsIdentity user)
        {
            var authType = "ActiveDirectory";
            string uniqueId = user.User.ToString();

            var ephUser = GetUser(authType, uniqueId);
            if (ephUser != null) { return ephUser; }

            var newUser = NewUser((short)AuthenticationEnum.ActiveDirectory);

            var windowsPrincipal = new WindowsPrincipal(user);
            var splitUser = user.Name.Split('\\');

            var newActiveDirectoryUser = new UserActiveDirectory();
            newActiveDirectoryUser.UserId = newUser.UserId;
            newActiveDirectoryUser.SID = uniqueId;
            newActiveDirectoryUser.UserName = user.Name.Split('\\').Last();
            newActiveDirectoryUser.Domain = splitUser.Count() > 1 ? splitUser[0] : "WORKGROUP";
            _db.Add(newActiveDirectoryUser);
            _db.SaveChanges();

            return GetUser(authType, uniqueId);
        }
        public Db.Models.User RegisterCurrent()
        {
            return RegisterActiveDirectory(WindowsIdentity.GetCurrent());
        }
        public Db.Models.User Register()
        {
            if(System.Environment.GetEnvironmentVariable("NOAUTH").Equals("True"))
            {
                return _db.User.Where(u => u.UserId == 1).FirstOrDefault();
            }
            if (!_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new AuthenticationException("User not authenticated");
            }

            if (_user != null) { return _user; }

            var userId = _httpContext.HttpContext.User.Identity;
            if (userId is WindowsIdentity)
            {
                _user = RegisterActiveDirectory((WindowsIdentity)userId);
            }
            return _user;
        }
        private ICollection<string> GetWindowsGroups()
        {
            var userId = (WindowsIdentity)_httpContext.HttpContext.User.Identity;
            var groupMembership = new HashSet<string>();
            foreach (var g in userId.Groups)
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
