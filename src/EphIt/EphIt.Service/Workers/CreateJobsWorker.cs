using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EphIt.Service.Posh.Job;
using EphIt.Db.Models;


//this is debug stuff
namespace EphIt.Service.Workers
{
    class CreateJobsWorker : BackgroundService
    {
        private readonly ILogger<StartPendingJobsWorker> _logger;
        private IPoshJobManager _poshJobManager;

        public CreateJobsWorker(ILogger<StartPendingJobsWorker> logger, IPoshJobManager poshJobManager)
        {
            _poshJobManager = poshJobManager;
            _logger = logger;
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
                _logger.LogInformation("Monitor Running Jobs at: {time}", DateTimeOffset.Now);
                PoshJob job1 = new PoshJob();
                job1.Script = TestScript;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("stringParam", "stringValue");
                parameters.Add("intParam", 1);
                job1.Parameters = parameters;
                _poshJobManager.QueuePendingJob(job1);
                await Task.Delay(10000, stoppingToken);
            }
        }
        private string TestScript = @"
param(
    [string]$stringParam,
    [int]$intParam
)
$VerbosePreference = continue
Write-Verbose -Message 'Verbose Output'
Write-Debug -Message 'Debug Output'
Write-Warning -Message 'Warning Output'
Write-Error -Message 'Error Output'
Write-Host -Object 'Host Output'
Write-Output -InputObject 'Output object'
Write-Output -InputObject([guid]::NewGuid());
Start-Sleep -Seconds 300            
";
    }
    
}

