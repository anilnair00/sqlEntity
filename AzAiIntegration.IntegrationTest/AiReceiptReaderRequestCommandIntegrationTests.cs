using AirCanada.Appx.Common.Enum;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{

    public class AiReceiptReaderRequestCommandIntegrationTests : IClassFixture<AzAiIntegrationTestFixture>
    {

        private readonly IServiceProvider _serviceProvider;
        public DateTime time = DateTime.Now.ToLocalTime();

        public AiReceiptReaderRequestCommandIntegrationTests(AzAiIntegrationTestFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        // The [Fact] attribute was removed because the Service Bus is no longer accessible in CI/DEV (pending migration from laptops to Cloud)
        private async Task E2E_SendRequestToServiceBus()
        {
            //Arrange
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var requestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();
            RequestEditInit(requestEdit);
            var requestCommand = dpFactory.GetPortal<AiReceiptReaderRequestCommand>().Create(requestEdit);

            //Act
            var result = await dpFactory.GetPortal<AiReceiptReaderRequestCommand>().ExecuteAsync(requestCommand);

            //Assert
            //ToDo: Assert should be on something returned by the Service Bus
            requestEdit.IsValid.Should().BeTrue();
        }

        private void RequestEditInit(ReceiptReaderRequestEdit requestEdit)
        {
            var time = DateTime.Now.ToLocalTime();
            requestEdit.CheckInDate = new SmartDate(new DateTime(2022, 1, 1));
            requestEdit.CheckOutDate = new SmartDate(new DateTime(2022, 1, 2));
            requestEdit.TransactionDate = new SmartDate(new DateTime(2022, 1, 3));

            requestEdit.CurrencyCode = "USD";
            requestEdit.CurrencySymbol = "$";
            requestEdit.DocumentFileName = $"{time} Test.pdf";
            requestEdit.DocumentLanguageCode = "en";
            requestEdit.DocumentSize = 12345;
            requestEdit.DocumentStorageIdentifier = "testIdentifier";
            requestEdit.DocumentStorageContainer = "testContainer";
            requestEdit.DocumentStoragePath = "testPath";

            requestEdit.ExpenseTypeCode = ExpenseTypeEnum.AN;
            requestEdit.TotalAmount = 100.0m;
        }
    }
}
