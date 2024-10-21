using AirCanada.Appx.Claim.Expense;
using AirCanada.Appx.Common.Extensions;
using Csla;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AirCanada.Appx.AzAiIntegration.Functions
{
    public class AiReceiptReaderRequestHandler
    {
        private readonly ILogger<AiReceiptReaderRequestHandler> _logger;
        private readonly IDataPortalFactory? _portalFactory;
        private readonly IConfiguration _configuration;

        public AiReceiptReaderRequestHandler(ILogger<AiReceiptReaderRequestHandler> logger, IDataPortalFactory? portalFactory, IConfiguration configuration)
        {
            _logger = logger;
            _portalFactory = portalFactory;
            _configuration = configuration;
        }

        [Function(nameof(AiReceiptReaderRequestHandler))]
        public async Task Run([BlobTrigger("cx-appx-claim-upload/{year}/{month}/{name}",
            Connection = "APPX-BlobStorage-ConnectionString")] Stream stream,
            string name)
        {
            _logger.LogInformation("Start {HandlerName} for blob\n Name: {BlobName}", nameof(AiReceiptReaderRequestHandler), name);

            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();

            var annotationId = ExtractAnnotationId(name);

            if (_portalFactory is null)
            {
                var errorMsg = "Portal Factory has not been initialized";
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestHandler), errorMsg, new ArgumentNullException(nameof(_portalFactory), errorMsg));
            }

            ExpenseReceiptDocumentInfo? expenseReceiptDocumentInfo = null;

            try
            {
                // Fetch Expense Claim details from PCAT SQL DB
                expenseReceiptDocumentInfo = await _portalFactory!.GetPortal<ExpenseReceiptDocumentInfo>().FetchAsync(annotationId);

                if (expenseReceiptDocumentInfo == null || expenseReceiptDocumentInfo.SsetOperationDocumentId == default)
                {
                    throw new InvalidOperationException($"ExpenseReceiptDocumentInfo not found for AnnotationId: {annotationId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestHandler), $"An error occurred while fetching expense claim details for AnnotationId: {annotationId}", ex);
            }

            _logger.LogInformation("{HandlerName} => Fetched {DocumentIdName}: {DocumentId} for AnnotationId: {AnnotationId}", nameof(AiReceiptReaderRequestHandler), nameof(expenseReceiptDocumentInfo.SsetOperationDocumentId), expenseReceiptDocumentInfo!.SsetOperationDocumentId, annotationId);

            // Create integration transaction in SQL APPX DB - [Integration].[AzAiIntegration] table
            ReceiptReaderRequestEdit? requestEdit = null;

            try
            {
                var receiptReaderRequestEdit = _portalFactory!.GetPortal<ReceiptReaderRequestEdit>().Create();
                ReceiptReaderRequestInit(receiptReaderRequestEdit, expenseReceiptDocumentInfo);

                requestEdit = receiptReaderRequestEdit.Save();
                _logger.LogInformation("{HandlerName} => Created AzAiIntegration record - {RecordIdName}: {RecordId}", nameof(AiReceiptReaderRequestHandler), nameof(requestEdit.Id), requestEdit.Id);
            }
            catch (Exception ex)
            {
                var errorMsg = $"An exception occurred while creating AzAiIntegration record for {nameof(expenseReceiptDocumentInfo.SsetOperationDocumentId)}: {expenseReceiptDocumentInfo.SsetOperationDocumentId}. Error: {ex.Message}";
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestHandler), errorMsg, ex);
            }

            try
            {
                SaveStageAndState(Common.Enum.StageEnum.Queuing, null, requestEdit!.Id);

                var aiReceiptReaderRequestCommand = _portalFactory!.GetPortal<AiReceiptReaderRequestCommand>().Create(requestEdit);
                await _portalFactory.GetPortal<AiReceiptReaderRequestCommand>().ExecuteAsync(aiReceiptReaderRequestCommand);
                SaveStageAndState(Common.Enum.StageEnum.Processing, Common.Enum.StateEnum.Queued, requestEdit.Id);
            }
            catch (Exception ex)
            {
                SaveStageAndState(null, Common.Enum.StateEnum.Failed, requestEdit!.Id);

                var errorMsg = $"An error occurred. Name of the document: {name}. Error: {ex.Message}";
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestHandler), errorMsg, ex);
            }
        }

        public void ReceiptReaderRequestInit(ReceiptReaderRequestEdit receiptReaderRequestEdit, ExpenseReceiptDocumentInfo expenseReceiptDocumentInfo)
        {

            var env = _configuration["ASPNETCORE_ENVIRONMENT"];
            var calibrationType = _configuration["CalibrationType"];

            if (env is null)
            {
                var errorMsg = "ASPNETCORE_ENVIRONMENT has not been initialized";
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestHandler), errorMsg, new ArgumentNullException("ASPNETCORE_ENVIRONMENT", errorMsg));
            }

            if (calibrationType is null)
            {
                var errorMsg = "CalibrationType has not been initialized";
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestHandler), errorMsg, new ArgumentNullException("CalibrationType", errorMsg));
            }

            receiptReaderRequestEdit.CreatedDateTime = DateTime.Now;
            receiptReaderRequestEdit.SsetDocumentId = expenseReceiptDocumentInfo.SsetOperationDocumentId;
            receiptReaderRequestEdit.DocumentFileName = expenseReceiptDocumentInfo.FileName;
            receiptReaderRequestEdit.DocumentLanguageId = expenseReceiptDocumentInfo.LanguageId;
            receiptReaderRequestEdit.DocumentLanguageCode = expenseReceiptDocumentInfo.LanguageCode;
            receiptReaderRequestEdit.CurrencyId = expenseReceiptDocumentInfo.CurrencyId;
            receiptReaderRequestEdit.CurrencySymbol = expenseReceiptDocumentInfo.CurrencySymbol;
            receiptReaderRequestEdit.CurrencyCode = expenseReceiptDocumentInfo.CurrencyCode;
            receiptReaderRequestEdit.DocumentSize = expenseReceiptDocumentInfo.Size;
            receiptReaderRequestEdit.DocumentStorageContainer = expenseReceiptDocumentInfo.StorageContainer;
            receiptReaderRequestEdit.DocumentStorageIdentifier = expenseReceiptDocumentInfo.StorageIdentifier;
            receiptReaderRequestEdit.DocumentStoragePath = expenseReceiptDocumentInfo.StoragePath;
            receiptReaderRequestEdit.ExpenseTypeCode = expenseReceiptDocumentInfo.ExpenseType;
            receiptReaderRequestEdit.TotalAmount = expenseReceiptDocumentInfo.TotalAmount;
            receiptReaderRequestEdit.TransactionDate = expenseReceiptDocumentInfo.TransactionDate;
            receiptReaderRequestEdit.CheckInDate = expenseReceiptDocumentInfo.CheckInDate;
            receiptReaderRequestEdit.CheckOutDate = expenseReceiptDocumentInfo.CheckOutDate;
            receiptReaderRequestEdit.DynamicExpenseWebRequestID = expenseReceiptDocumentInfo.DynamicExpenseWebRequestID;
            receiptReaderRequestEdit.DynamicsAnnotationWebRequestId = expenseReceiptDocumentInfo.DynamicsAnnotationWebRequestId;
            receiptReaderRequestEdit.SsetOperationId = expenseReceiptDocumentInfo.SsetOperationId;
            receiptReaderRequestEdit.SsetExpenseId = expenseReceiptDocumentInfo.SsetOperationExpenseId;
            receiptReaderRequestEdit.Stage = Common.Enum.StageEnum.Queuing;
            receiptReaderRequestEdit.State = Common.Enum.StateEnum.New;
            receiptReaderRequestEdit.Version = _configuration["Version"];
            receiptReaderRequestEdit.Environment = Enum.Parse<Common.Enum.EnvironmentEnum>(env!);
            receiptReaderRequestEdit.CalibrationType = Enum.Parse<Common.Enum.CalibrationTypeEnum>(calibrationType!);
            receiptReaderRequestEdit.CalibrationValue = _configuration["CalibrationValue"];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Used as an instance method by FACT")]
        public Guid ExtractAnnotationId(string fileName)
        {
            var regex = new Regex(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}");
            var match = regex.Match(fileName);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Failed to extract Dynamics Annotation GUID from blob name \n Name: {fileName}.");
            }

            var annotationId = Guid.Parse(match.Value);

            return annotationId;
        }

        private void SaveStageAndState(Common.Enum.StageEnum? stage, Common.Enum.StateEnum? state, long id)
        {
            try
            {
                // Fetch the existing ReceiptReaderRequestStateEdit object
                var receiptReaderRequestStateEdit = _portalFactory!.GetPortal<ReceiptReaderRequestStateEdit>().Fetch(id);

                // Conditionally update the Stage and State properties
                if (stage != null)
                {
                    receiptReaderRequestStateEdit.Stage = stage;
                }

                if (state != null)
                {
                    receiptReaderRequestStateEdit.State = state;
                }

                // Save the updated object
                receiptReaderRequestStateEdit = receiptReaderRequestStateEdit.Save();
            }
            catch (Exception ex)
            {
                _logger.LogAndThrow<AiReceiptReaderRequestHandler>(nameof(AiReceiptReaderRequestHandler), $"Error updating stage and state for ID {id}", ex);
            }
        }
    }
}