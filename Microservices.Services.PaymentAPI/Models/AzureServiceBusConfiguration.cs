
namespace Microservices.Services.PaymentAPI.Models;
public class AzureServiceBusConfiguration
{
    public string ServiceBusConnectionString { get; set; }
    public string OrderPaymentProcessTopic { get; set; }
    public string PaymentSubscription { get; set; }
    public string OrderPaymentResultTopic { get; set; }
}
