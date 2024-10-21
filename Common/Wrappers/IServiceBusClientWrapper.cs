using Azure.Messaging.ServiceBus;

namespace AirCanada.Appx.Common.Wrappers
{
    public interface IServiceBusClientWrapper
    {
        Task SendMessageAsync(string connectionString, string queueName, ServiceBusMessage message);
    }
}
