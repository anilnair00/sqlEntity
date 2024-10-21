using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public interface IAiReceiptReaderRequestDal
    {
        Task Insert(AiReceiptReaderRequestMsg aiReceiptReaderRequestMsg);
    }
}
