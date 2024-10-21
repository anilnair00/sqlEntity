using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.Common.Wrappers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class AiReceiptReaderRequestDalIntegrationTest
    {
        private readonly AiReceiptReaderRequestDalTestsMock _mock;
        private readonly IConfiguration _config;
        private readonly ILogger<AiReceiptReaderRequestDal> _logger;
        private readonly ILogger<ServiceBusClientWrapper> _serviceBuslogger;

        public AiReceiptReaderRequestDalIntegrationTest(AzAiIntegrationTestFixture fixture)
        {
            _mock = new AiReceiptReaderRequestDalTestsMock();
            _config = fixture.Configuration;
            _logger = fixture.ServiceProvider.GetRequiredService<ILogger<AiReceiptReaderRequestDal>>();
            _serviceBuslogger = fixture.ServiceProvider.GetRequiredService<ILogger<ServiceBusClientWrapper>>();
        }

        //ToDo: We need access to the Service Bus in CI/DEV to run this test
        private async Task SendRequestToServiceBus()
        {
            var dto = _mock.GenerateReceiptReaderRequestMsg();

            var dal = new AiReceiptReaderRequestDal(new ServiceBusClientWrapper(_serviceBuslogger), _config, _logger);
            await dal.Insert(dto);

            dto.Should().NotBeNull();
        }
    }
}