using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.BL.Authorization
{
    public interface IUserAuthorization
    {
        List<int> GetRoles();
        void SetAuthenticatedWith(int? roleId);
        int? AuthenticatedWith();
    }
}
