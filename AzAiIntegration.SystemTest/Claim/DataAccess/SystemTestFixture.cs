using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Common.Wrappers;
using Csla.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AirCanada.Appx.AzAiIntegration.SystemTest.Claim.DataAccess
{
    public class SystemTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public SystemTestFixture()
        {
            // Heads-up for Devs
            // To run the system tests locally, you will need to set your user secrets for this project.
            // Also, we will need to ensure that your network configuration allows you to access the CI/DEV SQL Databases.
            //
            // Building the configuration so that it reads from user secrets and environment variables
            var configurationBuilder = new ConfigurationBuilder()
                .AddUserSecrets<SystemTestFixture>(optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build the IConfiguration
            Configuration = configurationBuilder.Build();

            // Create a new ServiceCollection
            var serviceCollection = new ServiceCollection();

            // Add DALs and other services
            serviceCollection.AddTransient<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();

            // Inject real instances of IDapperWrapper and IDbConnection
            var claimConnectionString = Configuration["APPX-Claim-ConnectionString"];
            serviceCollection.AddTransient<IDbConnection>(provider => new SqlConnection(claimConnectionString));
            serviceCollection.AddTransient<IDapperWrapper, DapperWrapper>();

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