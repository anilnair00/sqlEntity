namespace AirCanada.Appx.Claim.DataAccess.Expense.Models
{
    public class SsetOperationExpenseEntity
    {
        public long Id { get; set; }
        public long SsetOperationId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencySymbol { get; set; }
        public Guid? DynamicExpenseWebRequestID { get; set; }
        public string? DynamicsExpenseRequest { get; set; }
        public string? DynamicsExpenseResponse { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? DisruptionCityAirportCode { get; set; }
        public int? MealTypeId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? TransportationTypeId { get; set; }
    }
}
