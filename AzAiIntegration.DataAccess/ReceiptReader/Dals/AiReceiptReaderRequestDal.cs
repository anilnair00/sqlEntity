using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.Common.Extensions;
using AirCanada.Appx.Common.Wrappers;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public class AiReceiptReaderRequestDal : IAiReceiptReaderRequestDal
    {
        private readonly IServiceBusClientWrapper _serviceBusClientWrapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AiReceiptReaderRequestDal> _logger;

        public AiReceiptReaderRequestDal(IServiceBusClientWrapper serviceBusClientWrapper, IConfiguration configuration, ILogger<AiReceiptReaderRequestDal> logger)
        {
            _serviceBusClientWrapper = serviceBusClientWrapper;
            _configuration = configuration;
            _logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task Insert(AiReceiptReaderRequestMsg aiReceiptReaderRequestMsg)
        {
            if (aiReceiptReaderRequestMsg is null)
            {
                var errorMsg = "The aiReceiptReaderRequestMsg cannot be null.";
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestDal), errorMsg, new ArgumentNullException(nameof(aiReceiptReaderRequestMsg)));
            }

            try
            {
                var connectionString = _configuration["APPX-AzureServiceBus-ConnectionString"];

                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogAndThrow(nameof(AiReceiptReaderRequestDal), "The 'APPX-AzureServiceBus-ConnectionString' configuration value cannot be null or empty.", new ArgumentNullException("APPX-AzureServiceBus-ConnectionString"));
                }

                var queueName = "receiptreaderrequest";

                var msgJson = JsonSerializer.Serialize(aiReceiptReaderRequestMsg!, _jsonSerializerOptions);

                _logger.LogInformation("{ClassName} => AI Service Request Payload: {Payload}", nameof(AiReceiptReaderRequestDal), msgJson);

                var message = new ServiceBusMessage(msgJson)
                {
                    SessionId = aiReceiptReaderRequestMsg!.MessageContext.RequestId.ToString(),
                    ScheduledEnqueueTime = DateTimeOffset.Now.AddSeconds(2)
                };

                await _serviceBusClientWrapper.SendMessageAsync(connectionString!, queueName, message);
            }
            catch (Exception ex)
            {
                _logger.LogAndThrow(nameof(AiReceiptReaderRequestDal), $"An error occurred while sending a request to the Service Bus. Error: {ex.Message}", ex);
            }
        }
    }
}