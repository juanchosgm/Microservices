
namespace Microservices.Services.OrderAPI.Models;
public class AzureServiceBusConfiguration
{
    public string ServiceBusConnectionString { get; set; }
    public string CheckoutMessageQueue { get; set; }
    public string CheckoutSubscription { get; set; }
    public string OrderPaymentProcessTopic { get; set; }
    public string OrderPaymentResultTopic { get; set; }
}
