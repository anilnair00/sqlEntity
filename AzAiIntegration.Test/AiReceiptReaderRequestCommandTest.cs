using AirCanada.Appx.Common.Enum;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public class AiReceiptReaderRequestCommandTest : IClassFixture<DiCslaFixture>
    {
        private readonly DiCslaFixture _diCslaFixture;
        private readonly IServiceProvider _serviceProvider;
        private readonly DateTime staticTime = new DateTime(2024, 10, 7, 16, 13, 58);

        public AiReceiptReaderRequestCommandTest(DiCslaFixture diCslaFixture)
        {
            _diCslaFixture = diCslaFixture;
            _serviceProvider = diCslaFixture.ServiceProvider;
        }

        [Fact]
        public void CreateBusinessObjectPassesValidation()
        {
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var requestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();
            RequestEditInit(requestEdit);

            var requestCommand = dpFactory.GetPortal<AiReceiptReaderRequestCommand>().Create(requestEdit);

            ValidateRequestEdit(requestCommand.ReceiptReaderRequestEdit);
        }

        [Fact]
        public async Task ExecuteInvalidObjectFails()
        {
            // Arrange
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var requestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();
            var requestCommand = dpFactory.GetPortal<AiReceiptReaderRequestCommand>().Create(requestEdit);

            // Act
            var exception = await Record.ExceptionAsync(() => dpFactory.GetPortal<AiReceiptReaderRequestCommand>().ExecuteAsync(requestCommand));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("DataPortal.Update failed (Exception of type 'Csla.Rules.ValidationException' was thrown.)");
        }

        [Fact]
        public async Task ShouldPrepareValidMsgPayload_WhenMsgSentToServiceBus()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var dpFactory = scopedServiceProvider.GetRequiredService<IDataPortalFactory>();

            var requestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();
            RequestEditInit(requestEdit);

            var aiReceiptReaderRequestCommand = dpFactory.GetPortal<AiReceiptReaderRequestCommand>().Create(requestEdit);

            // Act
            await dpFactory.GetPortal<AiReceiptReaderRequestCommand>().ExecuteAsync(aiReceiptReaderRequestCommand);

            // Assert
            _diCslaFixture.Should().NotBeNull("DiCslaFixture should be registered in the test constructor");

            var capturedMessage = _diCslaFixture!.CapturedMessage;
            capturedMessage.Should().NotBeNull("A message should have been captured");

            var messageBody = capturedMessage!.Body.ToString();
            messageBody.Should().NotContain("\"Stage\":");
            messageBody.Should().NotContain("\"State\":");

            // Additional assertions for expected message body elements
            messageBody.Should().Contain("\"Code\":\"USD\"");
            messageBody.Should().Contain("\"Symbol\":\"$\"");
            messageBody.Should().Contain("\"FileName\":\"2024-10-07T16:13:58 Test.pdf\"");
            messageBody.Should().Contain("\"LanguageCode\":\"en\"");
            messageBody.Should().Contain("\"Size\":12345");
            messageBody.Should().Contain("\"StorageIdentifier\":\"testIdentifier\"");
            messageBody.Should().Contain("\"StorageContainer\":\"testContainer\"");
            messageBody.Should().Contain("\"StoragePath\":\"testPath\"");
            messageBody.Should().Contain("\"ExpenseTypeCode\":\"AN\"");
            messageBody.Should().Contain("\"InputContent\":\"100.0\"");
        }

        private void RequestEditInit(ReceiptReaderRequestEdit requestEdit)
        {
            var time = DateTime.Now.ToLocalTime();
            requestEdit.CheckInDate = new SmartDate(new DateTime(2022, 1, 1));
            requestEdit.CheckOutDate = new SmartDate(new DateTime(2022, 1, 2));
            requestEdit.TransactionDate = new SmartDate(new DateTime(2022, 1, 3));

            requestEdit.CurrencyCode = "USD";
            requestEdit.CurrencySymbol = "$";
            requestEdit.DocumentFileName = $"{staticTime:yyyy-MM-ddTHH:mm:ss} Test.pdf";
            requestEdit.DocumentLanguageCode = "en";
            requestEdit.DocumentSize = 12345;
            requestEdit.DocumentStorageIdentifier = "testIdentifier";
            requestEdit.DocumentStorageContainer = "testContainer";
            requestEdit.DocumentStoragePath = "testPath";

            requestEdit.ExpenseTypeCode = ExpenseTypeEnum.AN;
            requestEdit.TotalAmount = 100.0m;
        }

        private void ValidateRequestEdit(ReceiptReaderRequestEdit requestEdit)
        {
            // This method centralizes the validation assertions for reuse
            requestEdit.CheckInDate.Should().Be(new SmartDate(new DateTime(2022, 1, 1)));
            requestEdit.CheckOutDate.Should().Be(new SmartDate(new DateTime(2022, 1, 2)));
            requestEdit.TransactionDate.Should().Be(new SmartDate(new DateTime(2022, 1, 3)));
            requestEdit.CurrencyCode.Should().Be("USD");
            requestEdit.CurrencySymbol.Should().Be("$");
            requestEdit.DocumentFileName.Should().Contain("Test.pdf");
            requestEdit.DocumentLanguageCode.Should().Be("en");
            requestEdit.DocumentSize.Should().Be(12345);
            requestEdit.DocumentStorageIdentifier.Should().Be("testIdentifier");
            requestEdit.DocumentStorageContainer.Should().Be("testContainer");
            requestEdit.DocumentStoragePath.Should().Be("testPath");
            requestEdit.ExpenseTypeCode.Should().Be(ExpenseTypeEnum.AN);
            requestEdit.TotalAmount.Should().Be(100.0m);
        }
    }
}
