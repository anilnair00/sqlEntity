using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.AzAiIntegration.Profiles;
using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Common.Wrappers;
using Csla.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class AzAiIntegrationTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public AzAiIntegrationTestFixture()
        {
            // Heads-up for Devs
            // To run the integration tests locally, you will need to set your user secrets for this project.
            // Also, we will need to ensure that your network configuration allows you to access the CI/DEV SQL Databases.
            //
            // Building the configuration so that it reads from user secrets and environment variables
            var configurationBuilder = new ConfigurationBuilder()
                .AddUserSecrets<AzAiIntegrationTestFixture>(optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build the IConfiguration
            Configuration = configurationBuilder.Build();

            // Create a new ServiceCollection
            var serviceCollection = new ServiceCollection();

            // Configure your services here
            serviceCollection.AddCsla();
            serviceCollection.AddAutoMapper(typeof(AzAiIntegrationMapper));

            // Add DALs and other services
            serviceCollection.AddSingleton<IServiceBusClientWrapper, ServiceBusClientWrapper>();
            serviceCollection.AddTransient<IAiReceiptReaderRequestDal, AiReceiptReaderRequestDal>();
            serviceCollection.AddTransient<IExpenseReceiptDocumentDal, ExpenseReceiptDocumentDal>();
            serviceCollection.AddTransient<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();
            serviceCollection.AddTransient<IReceiptReaderRequestEditDal, ReceiptReaderRequestEditDal>();

            var connectionString = Configuration["APPX-ConnectionString"];

            serviceCollection.AddDbContext<AppxDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Singleton);

            // Add logging
            serviceCollection.AddLogging(builder => builder.AddConsole());

            // Build the ServiceProvider
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
