using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.Common.Enum;
using AutoMapper;
using Csla;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirCanada.Appx.AzAiIntegration
{
    [Serializable]
    public class ReceiptReaderRequestEdit : BusinessBase<ReceiptReaderRequestEdit>
    {
        public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(c => c.Id);
        public long Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<StageEnum?> StageProperty = RegisterProperty<StageEnum?>(c => c.Stage);
        public StageEnum? Stage
        {
            get { return GetProperty(StageProperty); }
            set { SetProperty(StageProperty, value); }
        }

        public static readonly PropertyInfo<StateEnum?> StateProperty = RegisterProperty<StateEnum?>(c => c.State);
        public StateEnum? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }

        //temporary string, will be replaced by the actual response payload
        public static readonly PropertyInfo<string> ResponsePayloadProperty = RegisterProperty<string>(c => c.ResponsePayload);
        public string ResponsePayload
        {
            get { return GetProperty(ResponsePayloadProperty); }
            set { SetProperty(ResponsePayloadProperty, value); }
        }

        //Message context propreties
        public static readonly PropertyInfo<DateTimeOffset> CreatedDateTimeProperty = RegisterProperty<DateTimeOffset>(c => c.CreatedDateTime);
        public DateTimeOffset CreatedDateTime
        {
            get { return GetProperty(CreatedDateTimeProperty); }
            set { SetProperty(CreatedDateTimeProperty, value); }
        }

        public static readonly PropertyInfo<EnvironmentEnum> EnvironmentProperty = RegisterProperty<EnvironmentEnum>(c => c.Environment);
        public EnvironmentEnum Environment
        {
            get { return GetProperty(EnvironmentProperty); }
            set { SetProperty(EnvironmentProperty, value); }
        }

        public static readonly PropertyInfo<string?> VersionProperty = RegisterProperty<string?>(c => c.Version);
        public string? Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }

        public static readonly PropertyInfo<long> SsetOperationIdProperty = RegisterProperty<long>(c => c.SsetOperationId);
        public long SsetOperationId
        {
            get { return GetProperty(SsetOperationIdProperty); }
            set { SetProperty(SsetOperationIdProperty, value); }
        }

        public static readonly PropertyInfo<long> SsetExpenseIdProperty = RegisterProperty<long>(c => c.SsetExpenseId);
        public long SsetExpenseId
        {
            get { return GetProperty(SsetExpenseIdProperty); }
            set { SetProperty(SsetExpenseIdProperty, value); }
        }

        public static readonly PropertyInfo<long> SsetDocumentIdProperty = RegisterProperty<long>(c => c.SsetDocumentId);
        public long SsetDocumentId
        {
            get { return GetProperty(SsetDocumentIdProperty); }
            set { SetProperty(SsetDocumentIdProperty, value); }
        }

        public static readonly PropertyInfo<Guid> DynamicExpenseWebRequestIDProperty = RegisterProperty<Guid>(c => c.DynamicExpenseWebRequestID);
        public Guid DynamicExpenseWebRequestID
        {
            get { return GetProperty(DynamicExpenseWebRequestIDProperty); }
            set { SetProperty(DynamicExpenseWebRequestIDProperty, value); }
        }

        public static readonly PropertyInfo<Guid> DynamicsAnnotationWebRequestIdProperty = RegisterProperty<Guid>(c => c.DynamicsAnnotationWebRequestId);
        public Guid DynamicsAnnotationWebRequestId
        {
            get { return GetProperty(DynamicsAnnotationWebRequestIdProperty); }
            set { SetProperty(DynamicsAnnotationWebRequestIdProperty, value); }
        }

        public static readonly PropertyInfo<SmartDate> CheckInDateProperty = RegisterProperty<SmartDate>(c => c.CheckInDate);
        public SmartDate CheckInDate
        {
            get { return GetProperty(CheckInDateProperty); }
            set { SetProperty(CheckInDateProperty, value); }
        }

        public static readonly PropertyInfo<SmartDate> CheckOutDateProperty = RegisterProperty<SmartDate>(c => c.CheckOutDate);
        public SmartDate CheckOutDate
        {
            get { return GetProperty(CheckOutDateProperty); }
            set { SetProperty(CheckOutDateProperty, value); }
        }

        public static readonly PropertyInfo<string> CurrencyCodeProperty = RegisterProperty<string>(c => c.CurrencyCode);
        [Required]
        public string CurrencyCode
        {
            get { return GetProperty(CurrencyCodeProperty); }
            set { SetProperty(CurrencyCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> CurrencySymbolProperty = RegisterProperty<string>(c => c.CurrencySymbol);
        [Required]
        public string CurrencySymbol
        {
            get { return GetProperty(CurrencySymbolProperty); }
            set { SetProperty(CurrencySymbolProperty, value); }
        }

        public static readonly PropertyInfo<int> CurrencyIdProperty = RegisterProperty<int>(c => c.CurrencyId);
        [Required]
        public int CurrencyId
        {
            get { return GetProperty(CurrencyIdProperty); }
            set { SetProperty(CurrencyIdProperty, value); }
        }

        public static readonly PropertyInfo<int> LanguageIdProperty = RegisterProperty<int>(c => c.DocumentLanguageId);
        public int DocumentLanguageId
        {
            get { return GetProperty(LanguageIdProperty); }
            set { SetProperty(LanguageIdProperty, value); }
        }

        public static readonly PropertyInfo<string> DocumentFileNameProperty = RegisterProperty<string>(c => c.DocumentFileName);
        [Required]
        public string DocumentFileName
        {
            get { return GetProperty(DocumentFileNameProperty); }
            set { SetProperty(DocumentFileNameProperty, value); }
        }

        public static readonly PropertyInfo<string> DocumentLanguageCodeProperty = RegisterProperty<string>(c => c.DocumentLanguageCode);
        [Required]
        public string DocumentLanguageCode
        {
            get { return GetProperty(DocumentLanguageCodeProperty); }
            set { SetProperty(DocumentLanguageCodeProperty, value); }
        }

        public static readonly PropertyInfo<long> DocumentSizeProperty = RegisterProperty<long>(c => c.DocumentSize);
        [Required]
        public long DocumentSize
        {
            get { return GetProperty(DocumentSizeProperty); }
            set { SetProperty(DocumentSizeProperty, value); }
        }

        public static readonly PropertyInfo<string> DocumentStorageIdentifierProperty = RegisterProperty<string>(c => c.DocumentStorageIdentifier);
        [Required]
        public string DocumentStorageIdentifier
        {
            get { return GetProperty(DocumentStorageIdentifierProperty); }
            set { SetProperty(DocumentStorageIdentifierProperty, value); }
        }

        public static readonly PropertyInfo<string> DocumentStorageContainerProperty = RegisterProperty<string>(c => c.DocumentStorageContainer);
        [Required]
        public string DocumentStorageContainer
        {
            get { return GetProperty(DocumentStorageContainerProperty); }
            set { SetProperty(DocumentStorageContainerProperty, value); }
        }

        public static readonly PropertyInfo<string> DocumentStoragePathProperty = RegisterProperty<string>(c => c.DocumentStoragePath);
        [Required]
        public string DocumentStoragePath
        {
            get { return GetProperty(DocumentStoragePathProperty); }
            set { SetProperty(DocumentStoragePathProperty, value); }
        }

        public static readonly PropertyInfo<ExpenseTypeEnum> ExpenseTypeCodeProperty = RegisterProperty<ExpenseTypeEnum>(c => c.ExpenseTypeCode);
        [Required]
        public ExpenseTypeEnum ExpenseTypeCode
        {
            get { return GetProperty(ExpenseTypeCodeProperty); }
            set { SetProperty(ExpenseTypeCodeProperty, value); }
        }

        public static readonly PropertyInfo<decimal> TotalAmountProperty = RegisterProperty<decimal>(c => c.TotalAmount);
        [Required]
        public decimal TotalAmount
        {
            get { return GetProperty(TotalAmountProperty); }
            set { SetProperty(TotalAmountProperty, value); }
        }

        public static readonly PropertyInfo<SmartDate> TransactionDateProperty = RegisterProperty<SmartDate>(c => c.TransactionDate);
        public SmartDate TransactionDate
        {
            get { return GetProperty(TransactionDateProperty); }
            set { SetProperty(TransactionDateProperty, value); }
        }

        public static readonly PropertyInfo<CalibrationTypeEnum?> CalibrationTypeProperty = RegisterProperty<CalibrationTypeEnum?>(c => c.CalibrationType);
        public CalibrationTypeEnum? CalibrationType
        {
            get { return GetProperty(CalibrationTypeProperty); }
            set { SetProperty(CalibrationTypeProperty, value); }
        }

        public static readonly PropertyInfo<string?> CalibrationValueProperty = RegisterProperty<string?>(c => c.CalibrationValue);
        public string? CalibrationValue
        {
            get { return GetProperty(CalibrationValueProperty); }
            set { SetProperty(CalibrationValueProperty, value); }
        }

        [Insert]
        [SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "Used by CSLA data portal")]
        private void Insert([Inject] IReceiptReaderRequestEditDal dal, [Inject] IMapper mapper)
        {
            using (BypassPropertyChecks)
            {
                var dto = mapper.Map<ReceiptReaderRequestDto>(this);
                this.Id = dal.Insert(dto);
            }
        }

        [Create]
        [SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "Used by CSLA data portal")]
        private void Create()
        {
            BusinessRules.CheckRules();
        }
    }
}