using Serilog;

namespace ManagementSystem.Api.Common.Extensions;

public static class LogginExtension
{
    public static IServiceCollection AddLogginExtension(this IServiceCollection services, IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("starting server.");

        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        return services;
    }
}