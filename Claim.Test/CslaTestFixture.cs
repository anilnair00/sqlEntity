using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using Csla.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.Claim.Test
{
    public class CslaTestFixture : IDisposable
    {
        public CslaTestFixture()
        {
            // Create a new ServiceCollection
            var services = new ServiceCollection();

            // Configure your services here
            services.AddCsla();

            // TODO: Potentially mock the following
            services.AddTransient<IExpenseReceiptDocumentDal, ExpenseReceiptDocumentDal>();
            services.AddTransient<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();


            var configuration = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();

            Configuration = configuration;

            ServiceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider ServiceProvider { get; }

        public IConfiguration Configuration { get; }


        public void Dispose()
        {
            // Clean up resources if needed
        }
    }
}
