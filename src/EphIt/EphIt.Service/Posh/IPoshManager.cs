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
        public bool RunspaceAvailable();
        public PowerShell NewPowerShell();
        public PowerShell GetPowerShell();
        public void RetirePowerShell(PowerShell powershell);
        public int GetNumberOfRemainingPowerShell();


    }
}
