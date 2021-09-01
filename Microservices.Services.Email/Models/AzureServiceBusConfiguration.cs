
namespace Microservices.Services.EmailAPI.Models;
public class AzureServiceBusConfiguration
{
    public string ServiceBusConnectionString { get; set; }
    public string OrderPaymentResultTopic { get; set; }
    public string EmailSubscription { get; set; }
}
