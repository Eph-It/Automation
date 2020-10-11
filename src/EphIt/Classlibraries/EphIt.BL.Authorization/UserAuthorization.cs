using EphIt.BL.User;
using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
        List<int> _userRoles;
        private int? authenticatedWith = null;
        public List<int> GetRoles()
        {
            if (_userRoles == null)
            {
                _userRoles = _db.RoleMembershipUser.Where
                    (
                        p =>
                            p.UserId.Equals(_userObj.Register().UserId)
                    )
                    .Select(p => p.RoleId)
                    .ToList();
            }
            return _userRoles;
        }
        public void SetAuthenticatedWith(int? roleId)
        {
            authenticatedWith = roleId;
        }
        public int? AuthenticatedWith()
        {
            return authenticatedWith;
        }
    }
}
