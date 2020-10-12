using EphIt.Db.Enums;
using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EphIt.BL.Authorization
{
    public interface IUserAuthorization
    {
        Task<List<Role>> GetRolesAsync();
        Task<List<int>> GetRoleIdsAsync();
        void SetAuthenticatedWith(int? roleId);
        int? AuthenticatedWith();
        Task<AuthorizedObjects> GetAuthorizedScripts(int? scriptId = null, RBACActionsEnum? action = null);
    }
}
