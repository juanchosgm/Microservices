
using Microservices.MessageBus;

namespace Microservices.Services.PaymentAPI.Models.Dtos;
public class UpdatePaymentResultMessageDto : BaseMessage
{
    public Guid OrderId { get; set; }
    public bool Status { get; set; }
}
