namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models
{
    public class DocumentModel
    {
        public string? FileName { get; set; }
        public string? LanguageCode { get; set; }
        public long? Size { get; set; }
        public string? StorageIdentifier { get; set; }
        public string? StorageContainer { get; set; }
        public string? StoragePath { get; set; }
    }
}
