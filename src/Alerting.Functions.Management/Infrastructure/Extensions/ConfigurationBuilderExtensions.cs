using Alerting.Functions.Management.Functions.IncidentManagement;

namespace Alerting.Functions.Management.Infrastructure.Extensions;

internal static class ConfigurationBuilderExtensions
{
    internal static void AddCustomAppConfig(this IFunctionsConfigurationBuilder builder, IConfiguration config)
    {
        var appConfigConnectionString = config.GetConnectionString("AppConfig");

        if (!string.IsNullOrEmpty(appConfigConnectionString))
        {
            builder.ConfigurationBuilder.AddAzureAppConfiguration(opts =>
            {
                opts.Connect(appConfigConnectionString);
                opts.ConfigureRefresh(refreshOptions =>
                refreshOptions.Register("SentinelKey", refreshAll: true));
            });
        }
        else
        {
            throw new ArgumentException("App config connection string is null");
        }
    }

    internal static void AddCustomLogging(this IFunctionsHostBuilder builder, IConfiguration config)
    {
        var appInsightsConnectionstring = config.GetConnectionString("AppInsights");

        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", typeof(Startup).Assembly.GetName().Name)
            .Enrich.WithProperty("Environment", builder.GetContext().EnvironmentName)
            .WriteTo.Console()
            .WriteTo.ApplicationInsights(new TelemetryConfiguration { ConnectionString = appInsightsConnectionstring }, TelemetryConverter.Traces)
            .CreateLogger();

        builder.Services.AddLogging(logging => logging.AddSerilog(logger, true));
    }

    internal static void RegisterRefitClients(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddRefitClient<IAzureMonitorService>()
            .ConfigureHttpClient(c =>
            {               
                c.BaseAddress = new Uri("https://management.azure.com");
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30)
            }))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));


        builder.Services.AddRefitClient<IIncidentManagementService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://api.opsgenie.com/v2/alerts");
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30)
            }))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
    }
}
