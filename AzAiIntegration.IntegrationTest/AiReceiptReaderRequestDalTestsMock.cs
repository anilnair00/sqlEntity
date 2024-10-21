using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AirCanada.Appx.Common.Enum;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class AiReceiptReaderRequestDalTestsMock //: AiReceiptReaderRequestDal
    {
        public AiReceiptReaderRequestMsg GenerateReceiptReaderRequestMsg()
        {
            var mockMsg = new AiReceiptReaderRequestMsg
            {
                //CheckInDate = DateTime.Now.Date.AddDays(-1),
                // CheckOutDate = DateTime.Now.Date,
                Currency = new CurrencyModel
                {
                    Code = "CAD",
                    Symbol = "$"
                },
                Document = new DocumentModel
                {
                    FileName = "test.pdf",
                    LanguageCode = "en",
                    Size = 12345,
                    StorageIdentifier = "testIdentifier",
                    StorageContainer = "testContainer",
                    StoragePath = "testPath"
                },
                ExpenseTypeCode = ExpenseTypeEnum.AN,
                TotalAmount = new TotalAmountModel
                {
                    InputContent = "100",
                },
                TransactionDate = new RequestTransactionDateModel
                {
                    InputContent = DateTime.Now.Date.ToString("yyyy-MM-dd")
                },
                MessageContext = new RequestMessageContextModel
                {
                    RequestId = 123,
                    CreatedDateTime = DateTimeOffset.Now,
                    Environment = EnvironmentEnum.DEV,
                    Version = "1.00",
                    SsetOperationId = 456,
                    SsetExpenseId = 789,
                    SsetDocumentId = 101112,
                    DynamicExpenseWebRequestID = Guid.NewGuid(),
                    DynamicsAnnotationWebRequestId = Guid.NewGuid()
                }
            };
            return mockMsg;
        }
    }
}
