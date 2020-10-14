using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EphIt.Service.Services.Agent;
using EphIt.Service.Services.JobManager;
using EphIt.Service.Services.RunspaceManager;

namespace EphIt.Service.Workers
{
    public class StartPendingJobsWorker : BackgroundService
    {
        private readonly ILogger<StartPendingJobsWorker> _logger;
        private IRunspaceManager _runspaceManager;
        private IPSAgent _psAgent;

        public StartPendingJobsWorker(ILogger<StartPendingJobsWorker> logger, IRunspaceManager pSRunspace, IPSAgent pSAgent)
        {
            _runspaceManager = pSRunspace;
            _logger = logger;
            _psAgent = pSAgent;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Starting");
            


            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Service");
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing Service");
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //pretend a job exists
                _psAgent.StartPendingJob();
                //var runspace1 = _runspaceManager.GetRunspace();
                //if(runspace1 == null)
                //{
                //    _logger.LogInformation("Unable to get runspace!");
                //}
                _logger.LogInformation($"Runspaces left: {_runspaceManager.GetNumberOfRemainingRunspaces()}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
