using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos
{
    public class ReceiptReaderResponseDto
    {
        public ReceiptReaderResponseDto()
        {
            MessageContext = new ResponseMessageContextModel();
            TransactionDate = new ResponseTransactionDateModel();
            TotalAmount = new ResponseTotalAmountModel();
        }

        public long Id { get; set; }
        public string? AiResponseMessage { get; set; }
        public long? SsetDocumentId { get; set; }   
        public ResponseTransactionDateModel? TransactionDate { get; set; }
        public ResponseTotalAmountModel? TotalAmount { get; set; }
        public ResponseMessageContextModel? MessageContext { get; set; }
    }
}