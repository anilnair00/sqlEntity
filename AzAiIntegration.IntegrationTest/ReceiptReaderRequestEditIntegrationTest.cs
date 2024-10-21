using AirCanada.Appx.Common.Enum;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class ReceiptReaderRequestEditIntegrationTest : IClassFixture<AzAiIntegrationTestFixture>
    {
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;

        public ReceiptReaderRequestEditIntegrationTest(AzAiIntegrationTestFixture fixture)
        {
            _config = fixture.Configuration;
            _services = fixture.ServiceProvider;
        }

        [Fact]
        private void E2E_InsertToSqlAzAiIntegrationTable()
        {
            var dpFactory = _services.GetRequiredService<IDataPortalFactory>();
            var receiptReaderRequestEdit = dpFactory.GetPortal<ReceiptReaderRequestEdit>().Create();
            InitializeProperties(receiptReaderRequestEdit);

            var result = receiptReaderRequestEdit.Save();

            result.Id.Should().BeGreaterThan(0);
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
            requestEdit.SsetDocumentId = 138L;
            requestEdit.DynamicExpenseWebRequestID = Guid.NewGuid();
            requestEdit.DynamicsAnnotationWebRequestId = Guid.NewGuid();
            requestEdit.CheckInDate = DateTime.Now.Date;
            requestEdit.CheckOutDate = DateTime.Now.Date;
            requestEdit.DocumentLanguageId = 1;
            requestEdit.TransactionDate = DateTime.Now.Date;
        }
    }
}
