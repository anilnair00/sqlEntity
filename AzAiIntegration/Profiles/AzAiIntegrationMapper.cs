using AutoMapper;

namespace AirCanada.Appx.AzAiIntegration.Profiles
{
    public class AzAiIntegrationMapper
    {
        public static IMapper ConfigureObjectMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ReceiptReaderRequestEdit_ReceiptReaderRequestDto_Profile>();
                cfg.AddProfile<ReceiptReaderRequestEdit_ReceiptReaderRequestMsg_Profile>();
                cfg.AddProfile<ReceiptReaderResponseEdit_ReceiptReaderResponseDto_Profile>();
                cfg.AddProfile<ReceiptReaderResponseDto_ReceiptReaderResponseEdit_Profile>();
            });

            return config.CreateMapper();
        }
    }
}
