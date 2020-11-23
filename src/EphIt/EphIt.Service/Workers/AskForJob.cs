using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EphIt.Service.Posh.Job;
using EphIt.Db.Models;
using EphIt.BL.Automation;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EphIt.Service.Workers
{
    public class AskForJob : BackgroundService
    {
        private readonly ILogger<StartPendingJobsWorker> _logger;
        private IPoshJobManager _poshJobManager;
        private IAutomationHelper _automationHelper;
        public AskForJob(ILogger<StartPendingJobsWorker> logger, IPoshJobManager poshJobManager, IConfiguration config)
        {
            _poshJobManager = poshJobManager;
            _logger = logger;
            _automationHelper = new AutomationHelper();
            _automationHelper.SetServer(config.GetSection("ServerInfo:WebServer").Value);
            _automationHelper.SetPort(Int32.Parse(config.GetSection("ServerInfo:Port").Value));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Service Starting");
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
                string url = _automationHelper.GetUrl() + $"/api/Job/Queued?language={ScriptLanguageEnum.PowerShellCore}";
                VMScriptJob newJob = null;
                try
                {
                    newJob = _automationHelper.GetWebCall<VMScriptJob>(url);
                }
                catch (Exception e)
                {
                    if(!e.InnerException.Message.Equals("No connection could be made because the target machine actively refused it.")){
                        _logger.LogError(e, "Something went wrong trying to get queued job.");
                    }
                    else
                    {
                        _logger.LogInformation("Server is offline");
                    }
                }
                if (newJob != null)
                {
                    PoshJob job = new PoshJob();
                    job.JobUID = newJob.Job.JobUid;
                    job.Script = newJob.Script;
                    if (newJob.Parameters != null)
                    {
                        job.Parameters = newJob.Parameters;
                    }
                    if (job.Script != null)
                    {
                        _poshJobManager.QueuePendingJob(job);
                    }
                    else
                    {
                        _logger.LogWarning("No script version found.");
                    }
                }
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
