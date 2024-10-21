namespace AirCanada.Appx.Claim.DataAccess.Expense.Models
{
    public class SsetOperationDocumentEntity
    {
        public long Id { get; set; }
        public string? FileName { get; set; }
        public string? DynamicsName { get; set; }
        public long DocumentSize { get; set; }
        public int DocumentLanguageId { get; set; }
        public string? StorageIdentifier { get; set; }
        public string? StorageContainer { get; set; }
        public string? StoragePath { get; set; }
        public Guid? DynamicsAnnotationWebRequestId { get; set; }
        public string? DynamicsAnnotationRequest { get; set; }
        public string? DynamicsAnnotationResponse { get; set; }
        public long SsetOperationExpenseId { get; set; }
        public string? LanguageCode { get; set; }
    }
}
