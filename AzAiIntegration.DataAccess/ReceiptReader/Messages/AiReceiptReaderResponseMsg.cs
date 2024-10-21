using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages.Details;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages
{
    public class AiReceiptReaderResponseMsg
    {
        public AiReceiptReaderResponseMsg()
        {
            TransactionDate = new ResponseTransactionDateMsgDetail();
            TotalAmount = new ResponseTotalAmountMsgDetail();
            MessageContext = new ResponseMessageContextMsgDetail();
        }

        public ResponseTransactionDateMsgDetail? TransactionDate { get; set; }
        public ResponseTotalAmountMsgDetail? TotalAmount { get; set; }
        public ResponseMessageContextMsgDetail? MessageContext { get; set; }
    }
}