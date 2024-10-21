using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader.Dals
{
    public class DalTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public DalTestFixture()
        {
            var services = new ServiceCollection();

            // Add logging
            services.AddLogging(builder => builder.AddConsole());

            // Add DALs
            services.AddTransient<IReceiptReaderResponseDal, ReceiptReaderResponseDal>();

            // Register the DbContext with a fixed in-memory database name
            // 
            // The in-memory database is shared across tests within the same test class instance.
            // This means that data seeded in one test can persist and cause conflicts in subsequent tests.
            services.AddDbContext<AppxDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));

            // Build service provider
            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}