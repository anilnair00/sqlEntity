using AirCanada.Appx.Common.Enum;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages.Details
{
    public class ResponseMessageContextMsgDetail
    {
        public Guid? ResponseId { get; set; }
        public long? CorrelationId { get; set; }
        public string? ModelVersion { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public EnvironmentEnum? Environment { get; set; }
        public string? Error { get; set; }
    }
}
