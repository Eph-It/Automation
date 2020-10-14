using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using System.Threading;

namespace EphIt.Service.Services.Agent
{
    public interface IPSAgent
    {
        //public void StartJob(PowerShell powerShell, string Script);
        //public PowerShell NewPowershell();
        public void StartPendingJob();
    }
}
