
using Azure.Messaging.ServiceBus;
using Microservices.MessageBus;
using Microservices.Services.PaymentAPI.Models;
using Microservices.Services.PaymentAPI.Models.Dtos;
using Microsoft.Extensions.Options;
using PaymentProcessor;

namespace Microservices.Services.PaymentAPI.Messaging;
public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly IProcessPayment processPayment;
    private readonly IOptions<AzureServiceBusConfiguration> busServiceSection;
    private readonly IMessageBus messageBus;
    private readonly ServiceBusProcessor paymentProcessor;

    public AzureServiceBusConsumer(IProcessPayment processPayment,
        IOptions<AzureServiceBusConfiguration> busServiceSection,
        IMessageBus messageBus)
    {
        this.processPayment = processPayment;
        this.busServiceSection = busServiceSection;
        this.messageBus = messageBus;
        ServiceBusClient? client = new(busServiceSection.Value.ServiceBusConnectionString);
        paymentProcessor = client.CreateProcessor(busServiceSection.Value.OrderPaymentProcessTopic, busServiceSection.Value.PaymentSubscription);
    }

    public async Task Start()
    {
        paymentProcessor.ProcessMessageAsync += ProcessPayment;
        paymentProcessor.ProcessErrorAsync += ErrorHandlerAsync;
        await paymentProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await paymentProcessor.DisposeAsync();
    }

    private Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task ProcessPayment(ProcessMessageEventArgs args)
    {
        PaymentRequestMessageDto? payload = args.Message.Body.ToObjectFromJson<PaymentRequestMessageDto>();
        bool result = processPayment.PaymentProcessor();
        UpdatePaymentResultMessageDto? updatePaymentResultMessage = new()
        {
            Status = result,
            OrderId = payload.OrderId,
            Email = payload.Email
        };
        try
        {
            await messageBus.PublishMessage(updatePaymentResultMessage, busServiceSection.Value.OrderPaymentResultTopic);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
