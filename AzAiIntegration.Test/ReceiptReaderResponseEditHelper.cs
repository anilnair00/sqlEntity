using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages.Details;
using AirCanada.Appx.Common.Enum;
using NodaTime;
using System.Text.Json;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public static class ReceiptReaderResponseEditHelper
    {
        public static void MockInstance(ReceiptReaderResponseEdit instance)
        {
            var fakeResponse = new AiReceiptReaderResponseMsg
            {
                TransactionDate = new ResponseTransactionDateMsgDetail
                {
                    InputContent = new LocalDate(2023, 10, 1),
                    ExtractedContent = new LocalDate(2023, 10, 1),
                    IsFound = true,
                    Confidence = 0.99
                },
                TotalAmount = new ResponseTotalAmountMsgDetail
                {
                    InputContent = "250.00",
                    CalibrationType = "absolute",
                    CalibrationValue = "0.95",
                    ExtractedContent = "250.00",
                    IsFound = true,
                    Confidence = 0.99
                },
                MessageContext = new ResponseMessageContextMsgDetail
                {
                    ResponseId = Guid.NewGuid(),
                    CorrelationId = 98L,
                    ModelVersion = "1.0",
                    CreatedDateTime = DateTimeOffset.UtcNow,
                    Environment = EnvironmentEnum.DEV,
                    Error = null
                }
            };

            var fakeJson = JsonSerializer.Serialize(fakeResponse);
            instance.AiResponseMessage = fakeJson;
            instance.ResponseMessageContext_Responseid = fakeResponse.MessageContext.ResponseId;
            instance.ResponseMessageContext_CorrelationId = fakeResponse.MessageContext.CorrelationId;
            instance.ResponseMessageContext_ModelVersion = fakeResponse.MessageContext.ModelVersion;
            instance.ResponseMessageContext_CreatedDateTime = fakeResponse.MessageContext.CreatedDateTime;
            instance.ResponseMessageContext_Environment = fakeResponse.MessageContext.Environment;
            instance.ResponseMessageContext_Error = fakeResponse.MessageContext.Error;
            instance.TotalAmount_InputContent = fakeResponse.TotalAmount.InputContent;
            instance.TotalAmount_ExtractedContent = fakeResponse.TotalAmount.ExtractedContent;
            instance.TotalAmount_CalibrationType = fakeResponse.TotalAmount.CalibrationType;
            instance.TotalAmount_CalibrationValue = fakeResponse.TotalAmount.CalibrationValue;
            instance.TotalAmount_IsFound = fakeResponse.TotalAmount.IsFound;
            instance.TotalAmount_Confidence = fakeResponse.TotalAmount.Confidence;
            instance.TransactionDate_InputContent = fakeResponse.TransactionDate.InputContent?.ToDateTimeUnspecified();
            instance.TransactionDate_ExtractedContent = fakeResponse.TransactionDate.ExtractedContent?.ToDateTimeUnspecified();
            instance.TransactionDate_IsFound = fakeResponse.TransactionDate.IsFound;
            instance.TransactionDate_Confidence = fakeResponse.TransactionDate.Confidence;
        }
    }
}