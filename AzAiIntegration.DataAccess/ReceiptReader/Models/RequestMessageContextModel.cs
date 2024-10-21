using AirCanada.Appx.Common.Enum;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models
{
    public class RequestMessageContextModel
    {
        public long RequestId { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public EnvironmentEnum Environment { get; set; }
        public string? Version { get; set; }
        public long SsetOperationId { get; set; }
        public long SsetExpenseId { get; set; }
        public long SsetDocumentId { get; set; }
        public Guid DynamicExpenseWebRequestID { get; set; }
        public Guid DynamicsAnnotationWebRequestId { get; set; }
    }
}
