
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string _connectionString;

        public MessageBus()
        {
            _connectionString = StaticClass.MessageBusConnectionString;
        }
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };
            await sender.SendMessageAsync(serviceBusMessage);
            await client.DisposeAsync();
        }
    }
}
