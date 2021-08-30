using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microservices.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        private readonly IConfiguration configuration;

        public AzureServiceBusMessageBus(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task PublishMessage(BaseMessage input, string topicName)
        {
            await using ServiceBusClient? client = new(configuration["Azure:ServiceBusConnectionString"]);
            ServiceBusSender? sender = client.CreateSender(topicName);
            string? payload = JsonConvert.SerializeObject(input);
            ServiceBusMessage? message = new(payload)
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await sender.SendMessageAsync(message);
        }
    }
}
