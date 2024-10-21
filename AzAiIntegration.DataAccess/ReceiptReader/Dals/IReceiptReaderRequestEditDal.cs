using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public interface IReceiptReaderRequestEditDal
    {
        public long Insert(ReceiptReaderRequestDto requestDto);
    }
}
