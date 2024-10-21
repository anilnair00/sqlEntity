using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AutoMapper;

namespace AirCanada.Appx.AzAiIntegration.Profiles
{
    public class ReceiptReaderRequestEdit_ReceiptReaderRequestDto_Profile : Profile
    {
        public ReceiptReaderRequestEdit_ReceiptReaderRequestDto_Profile()
        {
            CreateMap<ReceiptReaderRequestEdit, ReceiptReaderRequestDto>()
                    .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => new CurrencyModel
                    {
                        Code = src.CurrencyCode,
                        Symbol = src.CurrencySymbol
                    }))
                    .ForMember(dest => dest.Document, opt => opt.MapFrom(src => new DocumentModel
                    {
                        FileName = src.DocumentFileName,
                        LanguageCode = src.DocumentLanguageCode,
                        Size = src.DocumentSize,
                        StorageIdentifier = src.DocumentStorageIdentifier,
                        StorageContainer = src.DocumentStorageContainer,
                        StoragePath = src.DocumentStoragePath,
                    }))
                    .ForMember(dest => dest.ExpenseTypeCode, opt => opt.MapFrom(src => src.ExpenseTypeCode))
                    .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => new TotalAmountModel
                    {
                        InputContent = src.TotalAmount.ToString(),
                        CalibrationType = src.CalibrationType,
                        CalibrationValue = src.CalibrationValue
                    }))
                    .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => new RequestTransactionDateModel
                    {
                        InputContent = src.TransactionDate.Date.ToString("yyyy-MM-dd")
                    }))
                    .ForMember(dest => dest.MessageContext, opt => opt.MapFrom(src => new RequestMessageContextModel
                    {
                        RequestId = src.Id,
                        CreatedDateTime = src.CreatedDateTime,
                        Environment = src.Environment,
                        Version = src.Version,
                        SsetOperationId = src.SsetOperationId,
                        SsetExpenseId = src.SsetExpenseId,
                        SsetDocumentId = src.SsetDocumentId,
                        DynamicExpenseWebRequestID = src.DynamicExpenseWebRequestID,
                        DynamicsAnnotationWebRequestId = src.DynamicsAnnotationWebRequestId
                    }))
                    .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => src.Stage))
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
        }
    }
}