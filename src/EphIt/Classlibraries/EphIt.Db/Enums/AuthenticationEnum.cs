using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    public enum AuthenticationEnum : short
    {
        ActiveDirectory = 1,
        EphItInternal = 2,
        AzureActiveDirectory = 3
    }
}
