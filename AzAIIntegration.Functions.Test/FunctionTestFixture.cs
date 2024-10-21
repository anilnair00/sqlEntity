using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using Csla.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirCanada.Appx.AzAiIntegration.Functions.Test
{
    public class FunctionTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public Mock<IConfiguration> ConfigurationMock { get; private set; }

        public FunctionTestFixture()
        {
            var services = new ServiceCollection();

            // Add CSLA
            services.AddCsla();

            // Add AutoMapper & DALs
            services.AddAutoMapper(typeof(ReceiptReaderResponseEdit).Assembly);
            services.AddScoped<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();
            services.AddScoped<IReceiptReaderRequestStateEditDal, ReceiptReaderRequestStateEditDal>();
            services.AddScoped<IReceiptReaderResponseDal, ReceiptReaderResponseDal>();

            // Add logging
            services.AddLogging(builder => builder.AddConsole());

            // Register the DbContext with a fixed in-memory database name
            services.AddDbContext<AppxDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));

            // Build service provider
            ServiceProvider = services.BuildServiceProvider();

            // Setup configuration mock
            ConfigurationMock = new Mock<IConfiguration>();
            ConfigurationMock.Setup(config => config["APPX-AzureServiceBus-ConnectionString"])
                .Returns("Endpoint=sb://svcbus-eus2-pcat-ai-dev-02.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ABC6DEfg4hIj2/kLmnOPq2RSTUvwxYZ4a+BCdEfgHIJ=");
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }
}