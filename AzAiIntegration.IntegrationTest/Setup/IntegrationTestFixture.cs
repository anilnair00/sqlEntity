using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.AzAiIntegration.Profiles;
using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Common.Wrappers;
using Csla.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzAiIntegration.IntegrationTest.Setup
{
    public class AzAiIntegrationTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public AzAiIntegrationTestFixture()
        {
            //Building the configuration so that it reads from local.settings.json and secrets.json (Secrets Overwrites the info)
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            configurationBuilder.AddUserSecrets<AzAiIntegrationTestFixture>(optional: true);

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

            var connectionString = Configuration["Values:APPX-ConnectionString"];

            serviceCollection.AddDbContext<AppxDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Singleton);

            var PCATconnectionString = Configuration["Values:APPX-Claim-ConnectionString"];
            var builder = new SqlConnectionStringBuilder(PCATconnectionString)
            {
                UserID = Configuration["Values:UserID"],
                MultipleActiveResultSets = false,
                Encrypt = true,
                TrustServerCertificate = false,
                Authentication = SqlAuthenticationMethod.ActiveDirectoryInteractive
            };

            // Build the ServiceProvider
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }
}
