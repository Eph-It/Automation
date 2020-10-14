using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;
using System.Collections.Concurrent;

namespace EphIt.Service.Services.RunspaceManager
{
    public class RunspaceManager : IRunspaceManager
    {
        private ConcurrentQueue<Runspace> _runspaceQueue;

        public RunspaceManager()
        {
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
    }
}
