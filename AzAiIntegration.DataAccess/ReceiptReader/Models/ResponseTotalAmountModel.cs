namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models
{
    public class ResponseTotalAmountModel
    {
        public string? InputContent { get; set; }
        public string? CalibrationType { get; set; }
        public string? CalibrationValue { get; set; }
        public string? ExtractedContent { get; set; }
        public bool? IsFound { get; set; }
        public double? Confidence { get; set; }
    }
}
