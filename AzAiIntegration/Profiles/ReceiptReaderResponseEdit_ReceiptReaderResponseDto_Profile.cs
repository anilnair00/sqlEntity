using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AutoMapper;
using NodaTime;

namespace AirCanada.Appx.AzAiIntegration.Profiles
{
    public class ReceiptReaderResponseEdit_ReceiptReaderResponseDto_Profile : Profile
    {
        public ReceiptReaderResponseEdit_ReceiptReaderResponseDto_Profile()
        {
            CreateMap<ReceiptReaderResponseEdit, ReceiptReaderResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AiResponseMessage, opt => opt.MapFrom(src => src.AiResponseMessage))
                .ForMember(dest => dest.MessageContext, opt => opt.MapFrom(src => new ResponseMessageContextModel
                {
                    Responseid = src.ResponseMessageContext_Responseid,
                    CorrelationId = src.ResponseMessageContext_CorrelationId,
                    ModelVersion = src.ResponseMessageContext_ModelVersion,
                    CreatedDateTime = src.ResponseMessageContext_CreatedDateTime,
                    Environment = src.ResponseMessageContext_Environment,
                    Error = src.ResponseMessageContext_Error
                }))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => new ResponseTotalAmountModel
                {
                    InputContent = src.TotalAmount_InputContent,
                    ExtractedContent = src.TotalAmount_ExtractedContent,
                    CalibrationType = src.TotalAmount_CalibrationType,
                    CalibrationValue = src.TotalAmount_CalibrationValue,
                    IsFound = src.TotalAmount_IsFound,
                    Confidence = src.TotalAmount_Confidence
                }))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => new ResponseTransactionDateModel
                {
                    InputContent = src.TransactionDate_InputContent.HasValue ? LocalDate.FromDateTime(src.TransactionDate_InputContent.Value) : default,
                    ExtractedContent = src.TransactionDate_ExtractedContent.HasValue ? LocalDate.FromDateTime(src.TransactionDate_ExtractedContent.Value) : default,
                    IsFound = src.TransactionDate_IsFound,
                    Confidence = src.TransactionDate_Confidence
                }));
        }
    }
}