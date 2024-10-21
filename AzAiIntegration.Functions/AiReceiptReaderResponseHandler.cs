using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.Claim.Expense;
using AirCanada.Appx.Common.Enum;
using AirCanada.Appx.Common.Extensions;
using Azure.Messaging.ServiceBus;
using Csla;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AirCanada.Appx.AzAiIntegration.Functions
{
    public class AiReceiptReaderResponseHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IDataPortalFactory _portalFactory;
        private readonly ILogger<AiReceiptReaderResponseHandler> _logger;

        public AiReceiptReaderResponseHandler(IConfiguration configuration, IDataPortalFactory portalFactory, ILogger<AiReceiptReaderResponseHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (configuration == null)
            {
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), "Configuration is null", new ArgumentNullException(nameof(configuration)));
            }

            if (portalFactory == null)
            {
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), "PortalFactory is null", new ArgumentNullException(nameof(portalFactory)));
            }

            _configuration = configuration!;
            _portalFactory = portalFactory!;
        }

        [Function(nameof(AiReceiptReaderResponseHandler))]
        public async Task Run([ServiceBusTrigger("receiptreaderresponse", Connection = "APPX-AzureServiceBus-ConnectionString", IsSessionsEnabled = true)] ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"Start {nameof(AiReceiptReaderResponseHandler)}");

            await ValidateServiceBusMessageAsync(message, messageActions);
            
            _logger.LogInformation("Started {HandlerName} for Message ID: {MessageId}", nameof(AiReceiptReaderResponseHandler), message!.MessageId);

            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            AiReceiptReaderResponseMsg? aiReceiptReaderResponseMsg = null;
            aiReceiptReaderResponseMsg = await DeserializeResponseMessageAsync(message!, messageActions);

            try
            {
                // Indicates that the Acknowledgement stage has begun 
                SaveStageAndState(StageEnum.Acknowledgement, state: null, aiReceiptReaderResponseMsg.MessageContext!.CorrelationId!.Value);

                var receiptReaderResponseEdit = await _portalFactory.GetPortal<ReceiptReaderResponseEdit>().FetchAsync(aiReceiptReaderResponseMsg.MessageContext!.CorrelationId);

                MapResponseMsgToBusinessObject(aiReceiptReaderResponseMsg, receiptReaderResponseEdit);
                receiptReaderResponseEdit.AiResponseMessage = Encoding.UTF8.GetString(message.Body);

                // Check for broken rules
                var brokenRules = receiptReaderResponseEdit.BrokenRulesCollection;
                if (brokenRules.Count > 0)
                {
                    // Convert broken rules to string
                    string brokenRulesString = brokenRules.ToString();
                    string errorMsg = $"Broken rules: {brokenRulesString}";
                    await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                    _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg);
                }

                // ToDo: Need to account for error handling scenarios (i.e. response received but failed/not completed successfully)
                //if (aiDocumentResponseModel.ResponseMessageContext_CorrelationId is not null)
                //{
                //    string errorMsg = $"Duplicate row exists.";
                //    await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                //    _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg);
                //}

                var receiptReaderResponseEdited = await receiptReaderResponseEdit.SaveAsync();

                if (!string.IsNullOrEmpty(aiReceiptReaderResponseMsg.MessageContext.Error))
                {
                    SaveStageAndState(null, Common.Enum.StateEnum.Failed, aiReceiptReaderResponseMsg.MessageContext.CorrelationId!.Value);

                    string errorMsg = $"AI Service Error: {aiReceiptReaderResponseMsg.MessageContext.Error}";
                    await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                    _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg);
                }

                await UpdatePcatTableResponse(receiptReaderResponseEdited, message, messageActions);

                await messageActions.CompleteMessageAsync(message);

                SaveStageAndState(null, Common.Enum.StateEnum.Processed, receiptReaderResponseEdited.Id);
            }
            catch (Exception ex)
            {
                SaveStageAndState(null, Common.Enum.StateEnum.Failed, aiReceiptReaderResponseMsg.MessageContext!.CorrelationId!.Value);

                string errorMsg = $"An error occurred while processing the Receipt Reader response message Id {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, ex);
            }
            finally
            {
                _logger.LogInformation("Completed {HandlerName} for Message ID: {MessageId}", nameof(AiReceiptReaderResponseHandler), message!.MessageId);
            }
        }

        private async Task<AiReceiptReaderResponseMsg> DeserializeResponseMessageAsync(ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            AiReceiptReaderResponseMsg? aiReceiptReaderResponseMsg = null;

            try
            {
                var messageBody = Encoding.UTF8.GetString(message.Body);

                // Configure JsonSerializerOptions to use NodaTime converters and handle enums
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

                aiReceiptReaderResponseMsg = JsonSerializer.Deserialize<AiReceiptReaderResponseMsg>(messageBody, options);

                if (aiReceiptReaderResponseMsg == null)
                {
                    var errorMsg = $"Failed to deserialize the message body for Message ID: {message.MessageId}.";
                    await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                    _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler),
                        errorMsg,
                        new ArgumentNullException(nameof(message.Body)));
                }
            }
            catch (JsonException jsonEx)
            {
                var errorMsg = $"Failed to deserialize the message body for Message ID: {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, jsonEx);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Unhandled exception for Message ID: {message.MessageId}";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, ex);
            }

            if (aiReceiptReaderResponseMsg?.MessageContext == null)
            {
                string? errorMsg = "Message context is null for Message ID: {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentNullException(nameof(aiReceiptReaderResponseMsg.MessageContext)));
            }

            if (aiReceiptReaderResponseMsg!.MessageContext!.CorrelationId == null)
            {
                string? errorMsg = "Missing correlation id (i.e. Request Id) for Message ID: {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentNullException(nameof(aiReceiptReaderResponseMsg.MessageContext.CorrelationId)));
            }

            return aiReceiptReaderResponseMsg;
        }

        private static bool IsEmptyJson(BinaryData body)
        {
            if (body == null || body.ToArray().Length == 0)
            {
                return true;
            }

            var messageBody = Encoding.UTF8.GetString(body);
            return string.IsNullOrWhiteSpace(messageBody) || messageBody.Trim() == "{}";
        }

        private void MapResponseMsgToBusinessObject(AiReceiptReaderResponseMsg? aiReceiptReaderResponseMsg, ReceiptReaderResponseEdit aiDocumentResponseEdit)
        {
            if (aiReceiptReaderResponseMsg == null)
            {
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), "Invalid response model.", new ArgumentNullException(nameof(aiReceiptReaderResponseMsg)));
            }

            if (aiReceiptReaderResponseMsg!.MessageContext != null)
            {
                aiDocumentResponseEdit.ResponseMessageContext_Responseid = aiReceiptReaderResponseMsg.MessageContext.ResponseId;
                aiDocumentResponseEdit.ResponseMessageContext_CorrelationId = aiReceiptReaderResponseMsg.MessageContext.CorrelationId;
                aiDocumentResponseEdit.ResponseMessageContext_ModelVersion = aiReceiptReaderResponseMsg.MessageContext.ModelVersion;
                aiDocumentResponseEdit.ResponseMessageContext_CreatedDateTime = aiReceiptReaderResponseMsg.MessageContext.CreatedDateTime;
                aiDocumentResponseEdit.ResponseMessageContext_Environment = aiReceiptReaderResponseMsg.MessageContext.Environment;
                aiDocumentResponseEdit.ResponseMessageContext_Error = aiReceiptReaderResponseMsg.MessageContext.Error;
            }

            if (aiReceiptReaderResponseMsg.TotalAmount != null)
            {
                aiDocumentResponseEdit.TotalAmount_InputContent = aiReceiptReaderResponseMsg.TotalAmount.InputContent;
                aiDocumentResponseEdit.TotalAmount_CalibrationType = aiReceiptReaderResponseMsg.TotalAmount.CalibrationValue;
                aiDocumentResponseEdit.TotalAmount_CalibrationValue = aiReceiptReaderResponseMsg.TotalAmount.CalibrationValue;
                aiDocumentResponseEdit.TotalAmount_ExtractedContent = aiReceiptReaderResponseMsg.TotalAmount.ExtractedContent;
                aiDocumentResponseEdit.TotalAmount_IsFound = aiReceiptReaderResponseMsg.TotalAmount.IsFound;
                aiDocumentResponseEdit.TotalAmount_Confidence = aiReceiptReaderResponseMsg.TotalAmount.Confidence;
            }

            if (aiReceiptReaderResponseMsg.TransactionDate != null)
            {
                aiDocumentResponseEdit.TransactionDate_InputContent = aiReceiptReaderResponseMsg.TransactionDate.InputContent?.ToDateTimeUnspecified();
                aiDocumentResponseEdit.TransactionDate_ExtractedContent = aiReceiptReaderResponseMsg.TransactionDate.ExtractedContent?.ToDateTimeUnspecified();
                aiDocumentResponseEdit.TransactionDate_IsFound = aiReceiptReaderResponseMsg.TransactionDate.IsFound;
                aiDocumentResponseEdit.TransactionDate_Confidence = aiReceiptReaderResponseMsg.TransactionDate.Confidence;
            }
        }

        private void MapPcatResponse(ReceiptReaderResponseEdit receiptReaderResponseEdit, ExpenseReceiptDocumentEdit expenseReceiptDocumentEdit)
        {
            if (receiptReaderResponseEdit is null)
            {
                _logger.LogAndThrow(nameof(AiReceiptReaderResponseHandler), "ReceiptReaderResponseEdit is null", new ArgumentNullException(nameof(receiptReaderResponseEdit)));
            }

            if (receiptReaderResponseEdit!.TotalAmount_InputContent is not null)
            {
                expenseReceiptDocumentEdit.IsValidAmount = receiptReaderResponseEdit.TotalAmount_IsFound;

                if (!string.IsNullOrEmpty(receiptReaderResponseEdit.TotalAmount_ExtractedContent))
                {
                    expenseReceiptDocumentEdit.ExtractedAmount = decimal.Parse(receiptReaderResponseEdit.TotalAmount_ExtractedContent);
                }
            }

            if (receiptReaderResponseEdit.TransactionDate_InputContent is not null)
            {
                expenseReceiptDocumentEdit.IsValidDate = receiptReaderResponseEdit.TransactionDate_IsFound;
                expenseReceiptDocumentEdit.ExtractedDate = receiptReaderResponseEdit.TransactionDate_ExtractedContent;
            }
        }

        private void SaveStageAndState(Common.Enum.StageEnum? stage, Common.Enum.StateEnum? state, long id)
        {
            try
            {
                // Fetch the existing ReceiptReaderRequestStateEdit object
                var receiptReaderRequestStateEdit = _portalFactory.GetPortal<ReceiptReaderRequestStateEdit>().Fetch(id);

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
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), $"Error updating stage and state for ID {id}", ex);
            }
        }

        private async Task UpdatePcatTableResponse(ReceiptReaderResponseEdit receiptReaderResponseEdit, ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            var expenseReceiptDocumentEdit = await _portalFactory.GetPortal<ExpenseReceiptDocumentEdit>().FetchAsync(receiptReaderResponseEdit.SsetDocumentId);

            MapPcatResponse(receiptReaderResponseEdit, expenseReceiptDocumentEdit);

            // Check for broken rules
            var brokenRules = receiptReaderResponseEdit.BrokenRulesCollection;
            if (brokenRules.Count > 0)
            {
                // Convert broken rules to string
                string brokenRulesString = brokenRules.ToString();
                string errorMsg = $"Broken rules: {brokenRulesString}";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg);
            }

            var saveResult = await expenseReceiptDocumentEdit.SaveAsync();

            // Log the validation results after saving
            _logger.LogInformation("PCAT table updated with expense receipt validation results: isValidAmount: {isValidAmount}, isValidDate: {isValidDate}", saveResult.IsValidAmount, saveResult.IsValidDate);
        }

        private async Task ValidateServiceBusMessageAsync(ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            if (message == null)
            {
                string errorMsg = $"Service Bus Message is null.";
                // Can't dead letter the message as the message is null (for testing only)
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentNullException(nameof(message)));
            }

            if (string.IsNullOrEmpty(message!.MessageId))
            {
                string errorMsg = $"Message ID is null or empty.";
                // Can't dead letter the message as the message id is null (for testing only)
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentNullException(nameof(message.MessageId)));
            }

            if (string.IsNullOrEmpty(message!.ContentType))
            {
                string errorMsg = $"Message content type is null or empty for Message ID: {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentNullException(nameof(message.ContentType)));
            }

            if (message.ContentType != "application/json")
            {
                string errorMsg = $"Message content type is not 'application/json' for Message ID: {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentException(nameof(message.ContentType)));
            }

            if (message!.Body is null || IsEmptyJson(message.Body))
            {
                string errorMsg = $"Message body is null for Message ID: {message.MessageId}.";
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: errorMsg);
                _logger.LogAndThrow<AiReceiptReaderResponseHandler>(nameof(AiReceiptReaderResponseHandler), errorMsg, new ArgumentNullException(nameof(message.Body)));
            }

            return;
        }
    }
}