using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;
using System.Collections.Concurrent;
using System.Management.Automation;
using EphIt.Service.Posh.Stream;
using System.Threading.Tasks;
using Serilog;

namespace EphIt.Service.Posh
{
    public class PoshManager : IPoshManager
    {
        private ConcurrentQueue<Runspace> _runspaceQueue;
        private ConcurrentQueue<PowerShell> _powerShellQueue;
        private IStreamHelper _streamHelper;

        public PoshManager(IStreamHelper streamHelper)
        {
            _streamHelper = streamHelper;
            _runspaceQueue = new ConcurrentQueue<Runspace>();
            _powerShellQueue = new ConcurrentQueue<PowerShell>();
            NewRunspaceQueue();
            NewPowershellQueue();
        }
        public Runspace NewRunspace()
        {
            //add environment stuff (modules etc, eventually) use sessionstate or whatever its called
            return RunspaceFactory.CreateRunspace();
        }
        public PowerShell NewPowerShell()
        {
            return PowerShell.Create();
        }
        public void NewRunspaceQueue()
        {
            for (int i = 0; i < 10; i++) //make this dynamic eventually.
            {
                _runspaceQueue.Enqueue(NewRunspace());
            }
        }
        public void NewPowershellQueue()
        {
            for (int i = 0; i < 10; i++) //make this dynamic eventually.
            {
                _powerShellQueue.Enqueue(NewPowerShell());
            }
        }
        public Runspace GetRunspace()
        {
            Runspace result;
            _runspaceQueue.TryDequeue(out result);
            if(result == null)
            {
                return null;
            }
            result.Open();
            return result;
        }
        public PowerShell GetPowerShell()
        {
            PowerShell result;
            _powerShellQueue.TryDequeue(out result);
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public void RetireRunspace(Runspace runspace)
        {
            runspace.Close();
            runspace.Dispose();
            //log something
            if(_runspaceQueue.Count < 10) //dynamic someday
            {
                _runspaceQueue.Enqueue(NewRunspace());
            }
        }
        public void RetirePowerShell(PowerShell powershell)
        {
            powershell.Dispose();
            if (_powerShellQueue.Count < 10) //dynamic someday
            {
                _powerShellQueue.Enqueue(NewPowerShell());
            }
        }
        public int GetNumberOfRemainingRunspaces()
        {
            return _runspaceQueue.Count;
        }
        public int GetNumberOfRemainingPowerShell()
        {
            return _powerShellQueue.Count;
        }
        private PowerShell GetPoshInstance(PoshJob poshJob/*, Runspace runspace*/) {
            PowerShell ps = GetPowerShell();
            ps.AddScript(poshJob.Script);
            //capture stream output
            ps.Streams.Verbose.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                _streamHelper.RecordStream(poshJob, sender, e);
            };
            ps.Streams.Debug.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                _streamHelper.RecordStream(poshJob, sender, e);
            };
            ps.Streams.Error.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                _streamHelper.RecordStream(poshJob, sender, e);
            };
            ps.Streams.Information.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                _streamHelper.RecordStream(poshJob, sender, e);
            };
            ps.Streams.Warning.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                _streamHelper.RecordStream(poshJob, sender, e);
            };
            return ps;
        }
        public PoshJob RunJob(PoshJob poshJob)
        {
            PowerShell ps = GetPoshInstance(poshJob);
            if (ps == null)
            {
                Log.Warning("All PowerShell instances are in use.");
                return null;
            }
            if (poshJob.Parameters != null && poshJob.Parameters.Count != 0)
            {
                ps.AddParameters(poshJob.Parameters);
            }
            
            poshJob.PoshInstance = ps;
            poshJob.RunningJob = StartScript(poshJob);
            Log.Information($"Job {poshJob.JobUID} started.");
            return poshJob;
        }
        
        public async Task<PSDataCollection<PSObject>> StartScript(PoshJob poshJob) //maybe add withRunspace parameter in the future. I have no idea what a runspace provides.
        {
            PSDataCollection<PSDataCollection<PSObject>> output = new PSDataCollection<PSDataCollection<PSObject>>();
            output.DataAdded += delegate (object sender, DataAddedEventArgs e) {
                _streamHelper.RecordOutput(poshJob, sender, e);
            };
            return await poshJob.PoshInstance.InvokeAsync<PSDataCollection<PSObject>, PSDataCollection<PSObject>>(null, output);
        }
        public bool RunspaceAvailable()
        {
            return !_runspaceQueue.IsEmpty;
        }
        public bool PowerShellAvailable()
        {
            return !_runspaceQueue.IsEmpty;
        }
        public void RecordOutput(Guid jobId, PSObject result)
        {
            //this might be its own class someday.
            Log.Information($"{jobId} Output: Type - {result.TypeNames[0]} Value - {result.BaseObject}");
        }
    }
}
