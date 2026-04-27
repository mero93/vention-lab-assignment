using Serilog;

namespace api.Extensions
{
    public static class LoggingConfiguration
    {
        public static void AddLoggerConfiguration(this IHostBuilder host)
        {
            host.UseSerilog(
                (context, services, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services);
                }
            );
        }
    }
}
