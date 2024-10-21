using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AutoMapper;

namespace AirCanada.Appx.AzAiIntegration.Profiles
{
    public class ReceiptReaderResponseDto_ReceiptReaderResponseEdit_Profile : Profile
    {
        public ReceiptReaderResponseDto_ReceiptReaderResponseEdit_Profile()
        {
            CreateMap<ReceiptReaderResponseDto, ReceiptReaderResponseEdit>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AiResponseMessage, opt => opt.MapFrom(src => src.AiResponseMessage))
                .ForMember(dest => dest.ResponseMessageContext_Responseid, opt => opt.MapFrom(src => src.MessageContext != null ? src.MessageContext.Responseid : null))
                .ForMember(dest => dest.ResponseMessageContext_CorrelationId, opt => opt.MapFrom(src => src.MessageContext != null ? src.MessageContext.CorrelationId : null))
                .ForMember(dest => dest.ResponseMessageContext_ModelVersion, opt => opt.MapFrom(src => src.MessageContext != null ? src.MessageContext.ModelVersion : null))
                .ForMember(dest => dest.ResponseMessageContext_CreatedDateTime, opt => opt.MapFrom(src => src.MessageContext != null ? src.MessageContext.CreatedDateTime : null))
                .ForMember(dest => dest.ResponseMessageContext_Environment, opt => opt.MapFrom(src => src.MessageContext != null ? src.MessageContext.Environment : null))
                .ForMember(dest => dest.ResponseMessageContext_Error, opt => opt.MapFrom(src => src.MessageContext != null ? src.MessageContext.Error : null))
                .ForMember(dest => dest.SsetDocumentId, opt => opt.MapFrom(src => src.SsetDocumentId != null ? src.SsetDocumentId : null))
                .ForMember(dest => dest.TotalAmount_InputContent, opt => opt.MapFrom(src => src.TotalAmount != null ? src.TotalAmount.InputContent : null))
                .ForMember(dest => dest.TotalAmount_ExtractedContent, opt => opt.MapFrom(src => src.TotalAmount != null ? src.TotalAmount.ExtractedContent : null))
                .ForMember(dest => dest.TotalAmount_CalibrationType, opt => opt.MapFrom(src => src.TotalAmount != null ? src.TotalAmount.CalibrationType : null))
                .ForMember(dest => dest.TotalAmount_CalibrationValue, opt => opt.MapFrom(src => src.TotalAmount != null ? src.TotalAmount.CalibrationValue : null))
                .ForMember(dest => dest.TotalAmount_IsFound, opt => opt.MapFrom(src => src.TotalAmount != null ? src.TotalAmount.IsFound : null))
                .ForMember(dest => dest.TotalAmount_Confidence, opt => opt.MapFrom(src => src.TotalAmount != null ? src.TotalAmount.Confidence : null))
                .ForMember(dest => dest.TransactionDate_InputContent, opt => opt.MapFrom(src => src.TransactionDate != null && src.TransactionDate.InputContent.HasValue ? src.TransactionDate.InputContent.Value.ToDateTimeUnspecified() : default))
                .ForMember(dest => dest.TransactionDate_ExtractedContent, opt => opt.MapFrom(src => src.TransactionDate != null && src.TransactionDate.ExtractedContent.HasValue ? src.TransactionDate.ExtractedContent.Value.ToDateTimeUnspecified() : default))
                .ForMember(dest => dest.TransactionDate_IsFound, opt => opt.MapFrom(src => src.TransactionDate != null ? src.TransactionDate.IsFound : null))
                .ForMember(dest => dest.TransactionDate_Confidence, opt => opt.MapFrom(src => src.TransactionDate != null ? src.TransactionDate.Confidence : null));
        }
    }
}