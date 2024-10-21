using AirCanada.Appx.AzAiIntegration.Functions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        if (hostContext.HostingEnvironment.EnvironmentName.Equals("Local"))
        {
            config.AddUserSecrets<Program>(optional: true, reloadOnChange: true);
        }

        // Load environment variables
        config.AddEnvironmentVariables();

        // Build the configuration here
        var builtConfig = config.Build();
        hostContext.Configuration = builtConfig;
    })
    .ConfigureServices((hostContext, services) =>
    {
        // Add Application Insights services
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Use the already built configuration
        services.AddConfigurations(hostContext);
    })
    .Build();

host.Run();