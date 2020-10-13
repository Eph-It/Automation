using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Db.Models
{
    public enum JobStatusEnum : short
    {
        New = 1,
        Queued = 2,
        InProgress = 3,
        Complete = 10,
        Error = 11,
        Cancelled = 12
    }
}
