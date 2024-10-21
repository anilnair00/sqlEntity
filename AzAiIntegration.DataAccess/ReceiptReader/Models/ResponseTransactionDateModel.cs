using NodaTime;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models
{
    public class ResponseTransactionDateModel
    {
        public LocalDate? InputContent { get; set; }
        public LocalDate? ExtractedContent { get; set; }
        public bool? IsFound { get; set; }
        public double? Confidence { get; set; }
    }
}
