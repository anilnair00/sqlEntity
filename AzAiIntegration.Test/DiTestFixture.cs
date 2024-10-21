using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using Csla.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public class DiTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public DiTestFixture()
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppxDbContext>(options => options.UseInMemoryDatabase("TestDb"));

            services.AddCsla();

            services.AddTransient<IExpenseReceiptDocumentDal, ExpenseReceiptDocumentDal>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }
}
