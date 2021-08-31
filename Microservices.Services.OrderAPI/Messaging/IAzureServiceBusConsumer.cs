
namespace Microservices.Services.OrderAPI.Messaging;
public interface IAzureServiceBusConsumer
{
    Task Start();
    ValueTask Stop();
}
