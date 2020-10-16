using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Threading.Tasks;

namespace EphIt.Service.Posh
{
    public interface IPoshManager
    {
        public Runspace GetRunspace();
        public void RetireRunspace(Runspace runspace);
        public int GetNumberOfRemainingRunspaces();
        public Task<PSDataCollection<PSObject>> RunJob(PoshJob poshJob);
        public bool RunspaceAvailable();
    }
}
