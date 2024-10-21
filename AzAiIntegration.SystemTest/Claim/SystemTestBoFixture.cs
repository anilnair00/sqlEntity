using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Common.Wrappers;
using Csla.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace AirCanada.Appx.AzAiIntegration.SystemTest.Claim
{
    public class SystemTestBoFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public SystemTestBoFixture()
        {
            var serviceCollection = new ServiceCollection();
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<SystemTestBoFixture>(optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.AddSingleton(Configuration);

            // Add other necessary services and configurations here
            serviceCollection.AddCsla();

            // Add DALs and other services
            serviceCollection.AddTransient<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();

            // Inject real instances of IDapperWrapper and IDbConnection
            var claimConnectionString = Configuration["APPX-Claim-ConnectionString"];
            serviceCollection.AddTransient<IDbConnection>(provider => new SqlConnection(claimConnectionString));
            serviceCollection.AddTransient<IDapperWrapper, DapperWrapper>();

            // Add logging
            serviceCollection.AddLogging(builder => builder.AddConsole());

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