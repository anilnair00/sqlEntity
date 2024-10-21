using AirCanada.Appx.Common.Extensions;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.Common.Wrappers
{
    public class ServiceBusClientWrapper : IServiceBusClientWrapper
    {
        private readonly ILogger<ServiceBusClientWrapper> _logger;

        public ServiceBusClientWrapper(ILogger<ServiceBusClientWrapper> logger)
        {
            _logger = logger;
        }

        public async Task SendMessageAsync(string connectionString, string queueName, ServiceBusMessage message)
        {
            try
            {
                await using var client = new ServiceBusClient(connectionString);
                var sender = client.CreateSender(queueName);
                await sender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                var errorMsg = $"An error occurred while sending a message to the Service Bus. Error: {ex.Message}";
                _logger.LogAndThrow<ServiceBusClientWrapper>(nameof(ServiceBusClientWrapper), errorMsg, ex);
            }
        }
    }
}