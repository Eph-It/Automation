using EphIt.Db.Enums;
using EphIt.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EphIt.Db.Models
{
    public class VMScriptJob
    {
        public Job Job { get; set; }
        public string Script { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
