
using Azure.Messaging.ServiceBus;
using Microservices.Services.EmailAPI.Models;
using Microservices.Services.EmailAPI.Models.Dtos;
using Microservices.Services.EmailAPI.Repository;
using Microsoft.Extensions.Options;

namespace Microservices.Services.EmailAPI.Messaging;
public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly EmailRepository emailRepository;
    private readonly IOptions<AzureServiceBusConfiguration> busServiceSection;
    private readonly ServiceBusProcessor orderUpdatePaymentStatusProcessor;

    public AzureServiceBusConsumer(EmailRepository emailRepository,
        IOptions<AzureServiceBusConfiguration> busServiceSection)
    {
        this.emailRepository = emailRepository;
        this.busServiceSection = busServiceSection;
        ServiceBusClient? client = new(busServiceSection.Value.ServiceBusConnectionString);
        orderUpdatePaymentStatusProcessor = client.CreateProcessor(busServiceSection.Value.OrderPaymentResultTopic, busServiceSection.Value.EmailSubscription);
    }

    public async Task Start()
    {
        orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
        orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandlerAsync;
        await orderUpdatePaymentStatusProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
        await orderUpdatePaymentStatusProcessor.DisposeAsync();
    }

    private Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs arg)
    {
        UpdatePaymentResultMessageDto? payload = arg.Message.Body.ToObjectFromJson<UpdatePaymentResultMessageDto>();
        try
        {
            await emailRepository.SendAndLogEmailAsync(payload);
            await arg.CompleteMessageAsync(arg.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
