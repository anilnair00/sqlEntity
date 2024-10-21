using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Common.Wrappers;
using Azure.Messaging.ServiceBus;
using Csla.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public class DiCslaFixture : IDisposable
    {
        public Mock<IConfiguration> ConfigurationMock { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public IServiceCollection Services { get; private set; }
        public Mock<IServiceBusClientWrapper> MockServiceBusClientWrapper { get; private set; }
        public ServiceBusMessage? CapturedMessage { get; private set; }

        public DiCslaFixture()
        {
            Services = new ServiceCollection();

            Services.AddCsla();

            // Add the required services for the DataAccess layer and other dependencies
            Services.AddAutoMapper(typeof(ReceiptReaderResponseEdit).Assembly);
            Services.AddScoped<IAiReceiptReaderRequestDal, AiReceiptReaderRequestDal>();
            Services.AddScoped<IReceiptReaderResponseDal, ReceiptReaderResponseDal>();
            Services.AddScoped<IServiceBusClientWrapper, ServiceBusClientWrapper>();

            // Setup configuration mock
            ConfigurationMock = new Mock<IConfiguration>();
            ConfigurationMock.Setup(config => config["APPX-AzureServiceBus-ConnectionString"])
                .Returns("Endpoint=sb://svcbus-eus2-pcat-ai-dev-02.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ABC6DEfg4hIj2/kLmnOPq2RSTUvwxYZ4a+BCdEfgHIJ=");

            // Register the mock configuration
            Services.AddSingleton<IConfiguration>(ConfigurationMock.Object);

            // Add a simple console logger for now
            Services.AddLogging(builder => builder.AddConsole());

            // Register the DbContext with a fixed in-memory database name
            Services.AddDbContext<AppxDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Scoped);

            // Setup the mock for IServiceBusClientWrapper
            MockServiceBusClientWrapper = new Mock<IServiceBusClientWrapper>();
            MockServiceBusClientWrapper
                .Setup(s => s.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ServiceBusMessage>()))
                .Callback<string, string, ServiceBusMessage>((conn, queue, msg) => CapturedMessage = msg)
                .Returns(Task.CompletedTask);

            // Register the mock in the service collection
            Services.AddSingleton(MockServiceBusClientWrapper.Object);

            ServiceProvider = Services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }
}