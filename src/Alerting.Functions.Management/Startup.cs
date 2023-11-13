[assembly: FunctionsStartup(typeof(Startup))]
namespace Alerting.Functions.Management;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        FunctionsHostBuilderContext context = builder.GetContext();

        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: false, reloadOnChange: true)
            .AddUserSecrets(Assembly.GetAssembly(typeof(Startup)))
            .AddEnvironmentVariables();
        
        var builtConfig = builder.ConfigurationBuilder.Build();
        builder.AddCustomAppConfig(builtConfig);
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;
        builder.Services.Configure<IncidentManagementConfiguration>(configuration.GetSection(nameof(IncidentManagementConfiguration)));
        builder.Services.Configure<ConfidentialClientConfiguration>(configuration.GetSection(nameof(ConfidentialClientConfiguration)));
        builder.RegisterRefitClients();
        builder.AddCustomLogging(configuration);
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorLoggingBehaviour<,>));
    }
}