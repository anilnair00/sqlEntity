using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages.Details;
using AirCanada.Appx.Common.Enum;
using NodaTime;

namespace AzAIIntegration.Functions.Test
{
    public static class MockResponseMessageHelper
    {
        public static AiReceiptReaderResponseMsg CreateMockResponseMessage(
            long correlationId = 98L,
            EnvironmentEnum environment = EnvironmentEnum.DEV)
        {
            return new AiReceiptReaderResponseMsg
            {
                MessageContext = new ResponseMessageContextMsgDetail
                {
                    CorrelationId = correlationId,
                    Environment = environment,
                    ResponseId = Guid.NewGuid(),
                    ModelVersion = "1.0",
                    CreatedDateTime = DateTimeOffset.UtcNow,
                    Error = null
                },
                TransactionDate = new ResponseTransactionDateMsgDetail
                {
                    InputContent = new LocalDate(2023, 10, 1),
                    ExtractedContent = new LocalDate(2023, 10, 1),
                    IsFound = true,
                    Confidence = 0.99
                },
                TotalAmount = new ResponseTotalAmountMsgDetail
                {
                    InputContent = "100.00",
                    ExtractedContent = "100.00",
                    IsFound = true,
                    Confidence = 0.99
                }
            };
        }
    }
}