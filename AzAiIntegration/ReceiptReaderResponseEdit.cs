using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.Common.Enum;
using AirCanada.Appx.Common.Extensions;
using AutoMapper;
using Csla;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace AirCanada.Appx.AzAiIntegration
{
    [Serializable]
    public class ReceiptReaderResponseEdit : BusinessBase<ReceiptReaderResponseEdit>
    {
        public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(nameof(Id));

        public long Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<string?> AiResponseMessageProperty = RegisterProperty<string?>(c => c.AiResponseMessage);
        [Required]
        public string? AiResponseMessage
        {
            get { return GetProperty(AiResponseMessageProperty); }
            set { SetProperty(AiResponseMessageProperty, value); }
        }

        public static readonly PropertyInfo<Guid?> ResponseMessageContext_ResponseidProperty = RegisterProperty<Guid?>(c => c.ResponseMessageContext_Responseid);
        [Required]
        public Guid? ResponseMessageContext_Responseid
        {
            get { return GetProperty(ResponseMessageContext_ResponseidProperty); }
            set { SetProperty(ResponseMessageContext_ResponseidProperty, value); }
        }

        public static readonly PropertyInfo<long?> ResponseMessageContext_CorrelationIdProperty = RegisterProperty<long?>(c => c.ResponseMessageContext_CorrelationId);
        [Required]
        public long? ResponseMessageContext_CorrelationId
        {
            get { return GetProperty(ResponseMessageContext_CorrelationIdProperty); }
            set { SetProperty(ResponseMessageContext_CorrelationIdProperty, value); }
        }

        public static readonly PropertyInfo<string?> ResponseMessageContext_ModelVersionProperty = RegisterProperty<string?>(c => c.ResponseMessageContext_ModelVersion);
        public string? ResponseMessageContext_ModelVersion
        {
            get { return GetProperty(ResponseMessageContext_ModelVersionProperty); }
            set { SetProperty(ResponseMessageContext_ModelVersionProperty, value); }
        }

        public static readonly PropertyInfo<DateTimeOffset?> ResponseMessageContext_CreatedDateTimeProperty = RegisterProperty<DateTimeOffset?>(c => c.ResponseMessageContext_CreatedDateTime);
        public DateTimeOffset? ResponseMessageContext_CreatedDateTime
        {
            get { return GetProperty(ResponseMessageContext_CreatedDateTimeProperty); }
            set { SetProperty(ResponseMessageContext_CreatedDateTimeProperty, value); }
        }

        public static readonly PropertyInfo<EnvironmentEnum?> ResponseMessageContext_EnvironmentProperty = RegisterProperty<EnvironmentEnum?>(c => c.ResponseMessageContext_Environment);
        [Required]
        public EnvironmentEnum? ResponseMessageContext_Environment
        {
            get { return GetProperty(ResponseMessageContext_EnvironmentProperty); }
            set { SetProperty(ResponseMessageContext_EnvironmentProperty, value); }
        }

        public static readonly PropertyInfo<string?> ResponseMessageContext_ErrorProperty = RegisterProperty<string?>(c => c.ResponseMessageContext_Error);
        public string? ResponseMessageContext_Error
        {
            get { return GetProperty(ResponseMessageContext_ErrorProperty); }
            set { SetProperty(ResponseMessageContext_ErrorProperty, value); }
        }

        public static readonly PropertyInfo<long> SsetDocumentIdProperty = RegisterProperty<long>(c => c.SsetDocumentId);
        [Required]
        public long SsetDocumentId
        {
            get { return GetProperty(SsetDocumentIdProperty); }
            set { SetProperty(SsetDocumentIdProperty, value); }
        }

        public static readonly PropertyInfo<string?> TotalAmount_InputContentProperty = RegisterProperty<string?>(c => c.TotalAmount_InputContent);
        [Required]
        public string? TotalAmount_InputContent
        {
            get { return GetProperty(TotalAmount_InputContentProperty); }
            set { SetProperty(TotalAmount_InputContentProperty, value); }
        }

        public static readonly PropertyInfo<string?> TotalAmount_CalibrationTypeProperty = RegisterProperty<string?>(c => c.TotalAmount_CalibrationType);
        public string? TotalAmount_CalibrationType
        {
            get { return GetProperty(TotalAmount_CalibrationTypeProperty); }
            set { SetProperty(TotalAmount_CalibrationTypeProperty, value); }
        }

        public static readonly PropertyInfo<string?> TotalAmount_CalibrationValueProperty = RegisterProperty<string?>(c => c.TotalAmount_CalibrationValue);
        public string? TotalAmount_CalibrationValue
        {
            get { return GetProperty(TotalAmount_CalibrationValueProperty); }
            set { SetProperty(TotalAmount_CalibrationValueProperty, value); }
        }

        public static readonly PropertyInfo<string?> TotalAmount_ExtractedContentProperty = RegisterProperty<string?>(c => c.TotalAmount_ExtractedContent);
        public string? TotalAmount_ExtractedContent
        {
            get { return GetProperty(TotalAmount_ExtractedContentProperty); }
            set { SetProperty(TotalAmount_ExtractedContentProperty, value); }
        }

        public static readonly PropertyInfo<bool?> TotalAmount_IsFoundProperty = RegisterProperty<bool?>(c => c.TotalAmount_IsFound);
        [Required]
        public bool? TotalAmount_IsFound
        {
            get { return GetProperty(TotalAmount_IsFoundProperty); }
            set { SetProperty(TotalAmount_IsFoundProperty, value); }
        }

        public static readonly PropertyInfo<double?> TotalAmount_ConfidenceProperty = RegisterProperty<double?>(c => c.TotalAmount_Confidence);
        public double? TotalAmount_Confidence
        {
            get { return GetProperty(TotalAmount_ConfidenceProperty); }
            set { SetProperty(TotalAmount_ConfidenceProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> TransactionDate_InputContentProperty = RegisterProperty<DateTime?>(c => c.TransactionDate_InputContent);
        [Required]
        public DateTime? TransactionDate_InputContent
        {
            get { return GetProperty(TransactionDate_InputContentProperty); }
            set { SetProperty(TransactionDate_InputContentProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> TransactionDate_ExtractedContentProperty = RegisterProperty<DateTime?>(c => c.TransactionDate_ExtractedContent);
        public DateTime? TransactionDate_ExtractedContent
        {
            get { return GetProperty(TransactionDate_ExtractedContentProperty); }
            set { SetProperty(TransactionDate_ExtractedContentProperty, value); }
        }

        public static readonly PropertyInfo<bool?> TransactionDate_IsFoundProperty = RegisterProperty<bool?>(c => c.TransactionDate_IsFound);
        public bool? TransactionDate_IsFound
        {
            get { return GetProperty(TransactionDate_IsFoundProperty); }
            set { SetProperty(TransactionDate_IsFoundProperty, value); }
        }

        public static readonly PropertyInfo<double?> TransactionDate_ConfidenceProperty = RegisterProperty<double?>(c => c.TransactionDate_Confidence);
        public double? TransactionDate_Confidence
        {
            get { return GetProperty(TransactionDate_ConfidenceProperty); }
            set { SetProperty(TransactionDate_ConfidenceProperty, value); }
        }

        [Update]
        private void Update([Inject] IReceiptReaderResponseDal dal, [Inject] ILogger<ReceiptReaderResponseEdit> logger, [Inject] IMapper mapper)
        {
            try
            {
                using (BypassPropertyChecks)
                {
                    var dto = mapper.Map<ReceiptReaderResponseDto>(this);
                    dal.Update(dto);
                }
            }
            catch (Exception ex)
            {
                {
                    var errorMsg = $"Failed to update Receipt Reader response. Response ID: {ResponseMessageContext_Responseid}, Correlation ID: {ResponseMessageContext_CorrelationId}";
                    logger.LogAndThrow<ReceiptReaderResponseEdit>(nameof(ReceiptReaderResponseEdit), errorMsg, ex);
                }
            }
        }

        [Fetch]
        private void Fetch([Inject] IReceiptReaderResponseDal dal, [Inject] ILogger<ReceiptReaderResponseEdit> logger, [Inject] IMapper mapper, long requestId)
        {
            try
            {
                var dto = dal.Fetch(requestId);

                if (dto != null)
                {
                    using (BypassPropertyChecks)
                    {
                        mapper.Map(dto, this);
                    }
                }

                BusinessRules.CheckRules();
            }
            catch (Exception ex)
            {
                var errorMsg = $"Failed to fetch document response for Request ID: {requestId}";
                logger.LogAndThrow<ReceiptReaderResponseEdit>(nameof(ReceiptReaderResponseEdit), errorMsg, ex);
            }
        }
    }
}
