namespace AirCanada.Appx.Claim.DataAccess.Expense.Dtos
{
    public class ExpenseReceiptDocumentEditDto
    {
        public ExpenseReceiptDocumentEditDto()
        {
            ExtractedDate = DateTime.Now;
        }

        public long? Id { get; set; }
        public decimal? ExtractedAmount { get; set; }
        public bool? IsValidAmount { get; set; }
        public bool? IsValidDate { get; set; }
        public DateTime? ExtractedDate { get; set; }
    }
}
