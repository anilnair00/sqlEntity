using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AirCanada.Appx.Common.Enum;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages
{
    public class AiReceiptReaderRequestMsg
    {
        public AiReceiptReaderRequestMsg()
        {
            Currency = new();

            Document = new();

            MessageContext = new()
            {
                CreatedDateTime = DateTime.Now,
            };

            TransactionDate = new();

            TotalAmount = new();
        }

        public RequestMessageContextModel MessageContext { get; set; }
        public CurrencyModel Currency { get; set; }
        public DocumentModel Document { get; set; }
        public ExpenseTypeEnum ExpenseTypeCode { get; set; }
        public TotalAmountModel TotalAmount { get; set; }
        public RequestTransactionDateModel TransactionDate { get; set; }
    }
}
