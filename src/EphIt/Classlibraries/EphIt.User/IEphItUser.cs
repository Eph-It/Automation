using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.User
{
    public interface IEphItUser
    {
        VUser Register();
        ICollection<string> GetGroupIds();
        VUser RegisterCurrent();
    }
}
