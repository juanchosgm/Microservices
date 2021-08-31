
namespace Microservices.Services.OrderAPI.Models;
public class AzureServiceBusConfiguration
{
    public string ServiceBusConnectionString { get; set; }
    public string CheckoutMessageTopic { get; set; }
    public string CheckoutSubscription { get; set; }
}
