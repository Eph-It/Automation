using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerShell;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Threading;
using EphIt.Service.Services.Agent;
using EphIt.Service.Services.JobManager;
using EphIt.Service.Services.RunspaceManager;
using EphIt.Db.Models;

namespace EphIt.Service.Services.Agent
{
    
    public class PSAgent : IPSAgent
    {
        private IRunspaceManager _runspaceManager;
        private IPowerShellJobManagerJobManager _jobManager;
        public PSAgent(IRunspaceManager runspaceManager, IPowerShellJobManagerJobManager jobManager)
        {
            _runspaceManager = runspaceManager;
            _jobManager = jobManager;
        }
        //public PowerShell NewPowershell()
        //{
        //    PowerShell ps = PowerShell.Create();
            //InitialSessionState iss = InitialSessionState.Create();
            //modules
            //need get-automationvariable module to be created here.  or maybe its cached?
            //other environment specific things.

            
            //ps.Runspace = rs;
            //return ps;
        //}
        public void StartPendingJob()
        {
            //is there a job?
            bool hasPendingJob = _jobManager.HasPendingJob();
            if (hasPendingJob)
            {
                _jobManager.StartPoshJob();
            }
        }
    }
}
