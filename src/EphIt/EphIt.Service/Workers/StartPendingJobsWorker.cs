using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EphIt.Service.Posh.Job;

namespace EphIt.Service.Workers
{
    public class StartPendingJobsWorker : BackgroundService
    {
        private readonly ILogger<StartPendingJobsWorker> _logger;
        private IPoshJobManager _poshJobManager;
        public StartPendingJobsWorker(ILogger<StartPendingJobsWorker> logger, IPoshJobManager poshJobManager)
        {
            _logger = logger;
            _poshJobManager = poshJobManager;
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
                _logger.LogInformation("Starting Pending Jobs running at: {time}", DateTimeOffset.Now);
                _poshJobManager.StartPendingJob();
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
