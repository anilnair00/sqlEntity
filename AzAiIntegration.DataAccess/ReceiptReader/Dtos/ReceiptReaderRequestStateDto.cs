namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos
{
    public class ReceiptReaderRequestStateDto
    {
        public long Id { get; set; }
        public string? Stage { get; set; }
        public string? State { get; set; }
    }
}