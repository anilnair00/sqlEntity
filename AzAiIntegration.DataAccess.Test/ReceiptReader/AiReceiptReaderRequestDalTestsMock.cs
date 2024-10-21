using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AirCanada.Appx.Common.Enum;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader
{
    public class AiReceiptReaderRequestDalTestsMock //: AiReceiptReaderRequestDal
    {
        public int id = 0;


        /*        public bool InsertCalled { get; private set; }
                public ReceiptReaderRequestDto InsertedDto { get; private set; }

                public new void Insert(ReceiptReaderRequestDto dto)
                {
                    InsertCalled = true;
                    InsertedDto = dto;
                }*/

        public ReceiptReaderRequestDto GenerateReceiptReaderRequestDto()
        {
            var mockDto = new ReceiptReaderRequestDto
            {
                // CheckInDate = DateTime.Now.Date.AddDays(-1),
                //CheckOutDate = DateTime.Now.Date,
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
                    InputContent = "100"
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
            return mockDto;
        }
    }
}
