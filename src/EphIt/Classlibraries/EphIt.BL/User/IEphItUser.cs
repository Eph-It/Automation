using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.BL.User
{
    public interface IEphItUser
    {
        Db.Models.User Register();
        ICollection<string> GetGroupIds();
        Db.Models.User RegisterCurrent();
    }
}
