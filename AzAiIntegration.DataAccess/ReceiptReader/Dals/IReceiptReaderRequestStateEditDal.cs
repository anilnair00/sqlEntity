using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public interface IReceiptReaderRequestStateEditDal
    {
        ReceiptReaderRequestStateDto Fetch(long id);
        void Update(ReceiptReaderRequestStateDto dto);
    }
}