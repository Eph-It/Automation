using EphIt.Db.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
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
                case "AzureActiveDirectory":
                    return _db.UserAzureActiveDirectory
                        .Where(p => p.ObjectId.Equals(uniqueIdentifier))
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
        private Db.Models.User NewActiveDirectoryUser(string SID, string UserName, string Domain)
        {
            var newUser = NewUser((short)AuthenticationEnum.ActiveDirectory);

            var newActiveDirectoryUser = new UserActiveDirectory();
            newActiveDirectoryUser.UserId = newUser.UserId;
            newActiveDirectoryUser.SID = SID;
            newActiveDirectoryUser.UserName = UserName;
            newActiveDirectoryUser.Domain = Domain;
            _db.Add(newActiveDirectoryUser);
            _db.SaveChanges();

            return GetUser("ActiveDirectory", SID);
        }
        private Db.Models.User RegisterActiveDirectory(WindowsIdentity user)
        {
            string uniqueId = user.User.ToString();
            var windowsPrincipal = new WindowsPrincipal(user);
            var splitUser = user.Name.Split('\\');

            var paramDictionary = new Dictionary<string, string>
            {
                { "SID", uniqueId },
                { "UserName", user.Name.Split('\\').Last() },
                { "Domain", splitUser.Count() > 1 ? splitUser[0] : "WORKGROUP" }
            };
            return Register("ActiveDirectory", paramDictionary);

        }
        private Db.Models.User NewAzureActiveDirectoryUser(string name, string userName, string objectId, string tenantId)
        {
            var newUser = NewUser((short)AuthenticationEnum.AzureActiveDirectory);
            var newAzureActiveDirectoryUser = new UserAzureActiveDirectory
            {
                Name = name,
                Email = userName,
                ObjectId = objectId,
                TenantId = tenantId,
                UserName = userName,
                UserId = newUser.UserId
            };
            _db.Add(newAzureActiveDirectoryUser);
            _db.SaveChanges();

            return GetUser("AzureActiveDirectory", objectId);
        }
        private Db.Models.User RegisterAzureActiveDirectory(ClaimsIdentity claimId)
        {
            string name = null;
            string userName = null;
            string objectId = null;
            string tenantId = null;
            foreach (var c in claimId.Claims)
            {
                switch (c.Type.ToLower())
                {
                    case "name":
                        name = c.Value;
                        break;
                    case "http://schemas.microsoft.com/identity/claims/objectidentifier":
                        objectId = c.Value;
                        break;
                    case "preferred_username":
                        userName = c.Value;
                        break;
                    case "http://schemas.microsoft.com/identity/claims/tenantid":
                        tenantId = c.Value;
                        break;
                }
            }
            if(String.IsNullOrEmpty(objectId) || String.IsNullOrEmpty(tenantId) || String.IsNullOrEmpty(userName))
            {
                throw new Exception("Error getting user information from Azure Active Directory claim");
            }
            var paramDictionary = new Dictionary<string, string>
            {
                { "Name", name },
                { "ObjectId", objectId },
                { "UserName", userName },
                { "TenantId", tenantId }
            };
            return Register("AzureActiveDirectory", paramDictionary);
        }
        public Db.Models.User RegisterCurrent()
        {
            return RegisterActiveDirectory(WindowsIdentity.GetCurrent());
        }
        public Db.Models.User Register()
        {
            var noAuth = System.Environment.GetEnvironmentVariable("NOAUTH");
            if (noAuth == "True")
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
            else if (userId.AuthenticationType == "AuthenticationTypes.Federation")
            {
                _user = RegisterAzureActiveDirectory((ClaimsIdentity)userId);
            }
            else
            {
                Log.Error("Unknown user type");
                throw new Exception("Unknown user type");
            }
            return _user;
        }
        public Db.Models.User Register(string AuthType, Dictionary<string, string> Values)
        {
            switch (AuthType)
            {
                case "ActiveDirectory":
                    string sid = Values["SID"];
                    var adUser = GetUser(AuthType, sid);
                    if (adUser != null) { return adUser; }
                    return NewActiveDirectoryUser(Values["SID"], Values["UserName"], Values["Domain"]);
                case "AzureActiveDirectory":
                    string objId = Values["ObjectId"];
                    var aadUser = GetUser(AuthType, objId);
                    if (aadUser != null) { return aadUser; }
                    return NewAzureActiveDirectoryUser(Values["Name"], Values["UserName"], Values["ObjectId"], Values["TenantId"]);
            }
            return null;
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
