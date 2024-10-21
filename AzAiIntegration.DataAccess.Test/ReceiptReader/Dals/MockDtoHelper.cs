using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AirCanada.Appx.Common.Enum;
using NodaTime;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader.Dals
{
    public static class MockDtoHelper
    {
        public static ReceiptReaderResponseDto CreateMockResponseDto(
            long correlationId = 98L,
            EnvironmentEnum environment = EnvironmentEnum.DEV)
        {
            return new ReceiptReaderResponseDto
            {
                Id = correlationId,
                AiResponseMessage = "Mock AI response message",
                MessageContext = new ResponseMessageContextModel
                {
                    CorrelationId = correlationId,
                    Environment = environment,
                    Responseid = Guid.NewGuid(),
                    ModelVersion = "1.0",
                    CreatedDateTime = DateTimeOffset.UtcNow,
                    Error = null
                },
                TransactionDate = new ResponseTransactionDateModel
                {
                    InputContent = new LocalDate(2023, 10, 1),
                    ExtractedContent = new LocalDate(2023, 10, 1),
                    IsFound = true,
                    Confidence = 0.99
                },
                TotalAmount = new ResponseTotalAmountModel
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