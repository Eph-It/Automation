using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace EphIt.Service
{
    public class PoshJob
    {
        public Guid JobUID { get; set; }
        public string Script { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public Task<PSDataCollection<PSObject>> RunningJob { get; set; }
        public PowerShell PoshInstance { get; set; }
    }
}
