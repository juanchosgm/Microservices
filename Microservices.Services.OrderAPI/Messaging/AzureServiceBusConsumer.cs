
using Azure.Messaging.ServiceBus;
using Microservices.MessageBus;
using Microservices.Services.OrderAPI.Models;
using Microservices.Services.OrderAPI.Models.Dtos;
using Microservices.Services.OrderAPI.Repository;
using Microsoft.Extensions.Options;

namespace Microservices.Services.OrderAPI.Messaging;
public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly OrderRepository orderRepository;
    private readonly IOptions<AzureServiceBusConfiguration> busServiceSection;
    private readonly IMessageBus messageBus;
    private readonly ServiceBusProcessor checkoutProcessor;
    private readonly ServiceBusProcessor orderUpdatePaymentStatusProcessor;

    public AzureServiceBusConsumer(OrderRepository orderRepository,
        IOptions<AzureServiceBusConfiguration> busServiceSection,
        IMessageBus messageBus)
    {
        this.orderRepository = orderRepository;
        this.busServiceSection = busServiceSection;
        this.messageBus = messageBus;
        ServiceBusClient? client = new(busServiceSection.Value.ServiceBusConnectionString);
        checkoutProcessor = client.CreateProcessor(busServiceSection.Value.CheckoutMessageQueue);
        orderUpdatePaymentStatusProcessor = client.CreateProcessor(busServiceSection.Value.OrderPaymentResultTopic, busServiceSection.Value.CheckoutSubscription);
    }

    public async Task Start()
    {
        checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceivedAsync;
        checkoutProcessor.ProcessErrorAsync += ErrorHandlerAsync;
        await checkoutProcessor.StartProcessingAsync();
        orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
        orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandlerAsync;
        await orderUpdatePaymentStatusProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await checkoutProcessor.StopProcessingAsync();
        await checkoutProcessor.DisposeAsync();
        await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
        await orderUpdatePaymentStatusProcessor.DisposeAsync();
    }

    private Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task OnCheckoutMessageReceivedAsync(ProcessMessageEventArgs args)
    {
        CheckoutHeaderDto? payload = args.Message.Body.ToObjectFromJson<CheckoutHeaderDto>();
        OrderHeader orderHeader = new()
        {
            UserId = payload.UserId,
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            OrderDetails = new List<OrderDetail>(),
            CardNumber = payload.CardNumber,
            CouponCode = payload.CouponCode,
            CVV = payload.CVV,
            DiscountTotal = payload.DiscountTotal,
            Email = payload.Email,
            ExpiryMonthYear = payload.ExpiryMonthYear,
            OrderTime = DateTime.UtcNow,
            OrderTotal = payload.OrderTotal,
            PaymentStatus = false,
            Phone = payload.Phone,
            PickupDateTime = payload.PickupDateTime
        };
        foreach (CartDetailDto? cartDetail in payload.CartDetails)
        {
            OrderDetail? orderDetail = new()
            {
                ProductId = cartDetail.ProductId,
                ProductName = cartDetail.Product.Name,
                Price = cartDetail.Product.Price,
                Count = cartDetail.Count
            };
            orderHeader.CartTotalItems += orderDetail.Count;
            orderHeader.OrderDetails.Add(orderDetail);
        }
        await orderRepository.AddOrderAsync(orderHeader);
        PaymentRequestMessageDto paymentRequestMessage = new()
        {
            Name = $"{orderHeader.FirstName} {orderHeader.LastName}",
            CardNumber = orderHeader.CardNumber,
            CVV = orderHeader.CVV,
            ExpiryMonthYear = orderHeader.ExpiryMonthYear,
            OrderId = orderHeader.OrderHeaderId,
            OrderTotal = orderHeader.OrderTotal,
        };
        try
        {
            await messageBus.PublishMessage(paymentRequestMessage, busServiceSection.Value.OrderPaymentProcessTopic);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs arg)
    {
        UpdatePaymentResultMessageDto? payload = arg.Message.Body.ToObjectFromJson<UpdatePaymentResultMessageDto>();
        await orderRepository.UpdateOrderPaymentStatus(payload.OrderId, payload.Status);
        await arg.CompleteMessageAsync(arg.Message);
    }
}
