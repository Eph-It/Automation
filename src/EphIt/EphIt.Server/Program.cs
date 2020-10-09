using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EphIt.Blazor.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tempLocation = Environment.GetEnvironmentVariable("TEMP");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    $"{tempLocation}\\EphIt.log"
                    ,rollOnFileSizeLimit: true
                    ,rollingInterval: RollingInterval.Day
                    ,retainedFileCountLimit: 14
                )
                .CreateLogger();

            Log.Information("Starting...");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
