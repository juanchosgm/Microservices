
using Azure.Messaging.ServiceBus;
using Microservices.Services.OrderAPI.Models;
using Microservices.Services.OrderAPI.Models.Dtos;
using Microservices.Services.OrderAPI.Repository;
using Microsoft.Extensions.Options;

namespace Microservices.Services.OrderAPI.Messaging;
public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly OrderRepository orderRepository;
    private readonly ServiceBusProcessor checkoutProcessor;

    public AzureServiceBusConsumer(OrderRepository orderRepository,
        IOptions<AzureServiceBusConfiguration> busServiceSection)
    {
        this.orderRepository = orderRepository;
        var client = new ServiceBusClient(busServiceSection.Value.ServiceBusConnectionString);
        checkoutProcessor = client.CreateProcessor(busServiceSection.Value.CheckoutMessageTopic, busServiceSection.Value.CheckoutSubscription);
    }

    public async Task Start()
    {
        checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceivedAsync;
        checkoutProcessor.ProcessErrorAsync += ErrorHandlerAsync;
        await checkoutProcessor.StartProcessingAsync();
    }

    public async ValueTask Stop()
    {
        await checkoutProcessor.DisposeAsync();
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
    }
}
