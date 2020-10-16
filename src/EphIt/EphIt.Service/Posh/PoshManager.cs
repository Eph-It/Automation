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
        private IStreamHelper _streamHelper;

        public PoshManager(IStreamHelper streamHelper)
        {
            _streamHelper = streamHelper;
            _runspaceQueue = new ConcurrentQueue<Runspace>();
            NewRunspaceQueue();
        }
        public Runspace NewRunspace()
        {
            //add environment stuff (modules etc, eventually) use sessionstate or whatever its called
            return RunspaceFactory.CreateRunspace();
        }
        public void NewRunspaceQueue()
        {
            for (int i = 0; i < 10; i++) //make this dynamic eventually.
            {
                _runspaceQueue.Enqueue(NewRunspace());
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
        public int GetNumberOfRemainingRunspaces()
        {
            return _runspaceQueue.Count;
        }
        private PowerShell GetPosh(PoshJob poshJob, Runspace runspace) {
            PowerShell ps = PowerShell.Create();
            ps.Runspace = runspace;
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
        public Task<PSDataCollection<PSObject>> RunJob(PoshJob poshJob)
        {
            Runspace rs = GetRunspace();
            if(rs == null)
            {
                //log here
                return null;
            }
            PowerShell ps = GetPosh(poshJob, rs);
            return ps.InvokeAsync<PSDataCollection<PSObject>>(poshJob.Script);
        }
        public bool RunspaceAvailable()
        {
            return !_runspaceQueue.IsEmpty;
        }
    }
}
