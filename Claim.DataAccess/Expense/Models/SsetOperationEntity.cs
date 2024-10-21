namespace AirCanada.Appx.Claim.DataAccess.Expense.Models
{
    public class SsetOperationEntity
    {
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? SessionId { get; set; }
        public string? OperationType { get; set; }
        public string? PNR { get; set; }
        public string? TicketNumber { get; set; }
        public DateTime? FlightDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? AssessmentCorrelationId { get; set; }
        public Guid? DynamicsWebRequestID { get; set; }
        public bool IsSsetV2 { get; set; }
        public string? DynamicsWebRequest { get; set; }
        public string? DynamicsWebResponse { get; set; }
        public Guid? OriginDestinationCorrelationId { get; set; }
    }
}
