using AirCanada.Appx.Common.Enum;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public class ReceiptReaderRequestEditTest : IClassFixture<DiTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        public DateTime time = DateTime.Now.ToLocalTime();

        public ReceiptReaderRequestEditTest(DiTestFixture diTestFixture)
        {
            _serviceProvider = diTestFixture.ServiceProvider;
        }

        [Fact]
        public void InsertInvalidObjectFails()
        {
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var requestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();

            var exception = Record.Exception(() => requestEdit.Save());

            exception.Should().NotBeNull();
            exception.Should().BeOfType<Csla.Rules.ValidationException>();
            requestEdit.IsValid.Should().BeFalse();
        }

        private void EndToEndInsertToSQLAzAiIntegrationTable()
        {
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var requestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();
            InitializeProperties(requestEdit);

            var result = requestEdit.Save();

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);

            false.Should().BeTrue("because this test should not run as it should not have a [Fact] attribute.");
        }

        private static void InitializeProperties(ReceiptReaderRequestEdit requestEdit)
        {
            requestEdit.CurrencyCode = "USD";
            requestEdit.CurrencySymbol = "$";
            requestEdit.CurrencyId = 1;
            requestEdit.DocumentFileName = "default.pdf";
            requestEdit.DocumentLanguageCode = "EN";
            requestEdit.DocumentSize = 0L;
            requestEdit.DocumentStorageIdentifier = "Temp1";
            requestEdit.DocumentStorageContainer = "Temp 2";
            requestEdit.DocumentStoragePath = "Temp 3";
            requestEdit.ExpenseTypeCode = ExpenseTypeEnum.AN;
            requestEdit.TotalAmount = 0M;

            // Initializing other properties
            requestEdit.Id = 0;
            requestEdit.Stage = StageEnum.Queuing;
            requestEdit.State = StateEnum.Queued;
            requestEdit.ResponsePayload = string.Empty;
            requestEdit.CreatedDateTime = DateTimeOffset.Now;
            requestEdit.Environment = EnvironmentEnum.DEV;
            requestEdit.Version = null;
            requestEdit.SsetOperationId = 0L;
            requestEdit.SsetExpenseId = 0L;
            requestEdit.SsetDocumentId = 0L;
            requestEdit.DynamicExpenseWebRequestID = Guid.NewGuid();
            requestEdit.DynamicsAnnotationWebRequestId = Guid.NewGuid();
            requestEdit.CheckInDate = DateTime.Now.Date;
            requestEdit.CheckOutDate = DateTime.Now.Date;
            requestEdit.DocumentLanguageId = 1;
            requestEdit.TransactionDate = DateTime.Now.Date;
        }
    }
}