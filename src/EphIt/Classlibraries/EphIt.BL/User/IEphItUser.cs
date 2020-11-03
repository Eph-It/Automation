using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.BL.User
{
    public interface IEphItUser
    {
        Db.Models.User Register();
        Db.Models.User Register(string AuthType, Dictionary<string, string> Values);
        ICollection<string> GetGroupIds();
        Db.Models.User RegisterCurrent();
    }
}
