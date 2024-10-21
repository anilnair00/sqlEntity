namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models
{
    public class TotalAmountModel
    {
        public string? InputContent { get; set; }
        public Common.Enum.CalibrationTypeEnum? CalibrationType { get; set; }
        public string? CalibrationValue { get; set; }
    }
}
