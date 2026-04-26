using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace api.Extensions
{
    public static class LoggingConfiguration
    {
        public static void AddLoggerConfiguration(this ConfigureHostBuilder builder)
        {
            builder.UseSerilog(
                (context, services, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File(
                            "logs/log-.txt",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7
                        );
                }
            );
        }
    }
}
