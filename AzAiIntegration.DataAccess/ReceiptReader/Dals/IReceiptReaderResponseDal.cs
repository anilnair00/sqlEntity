using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public interface IReceiptReaderResponseDal
    {
        public void Update(ReceiptReaderResponseDto aiDocumentResponseDto);
        public ReceiptReaderResponseDto? Fetch(long requestId);
    }
}
