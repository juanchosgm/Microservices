
namespace Microservices.Services.OrderAPI.Models.Dtos;
public class UpdatePaymentResultMessageDto
{
    public Guid OrderId { get; set; }
    public bool Status { get; set; }
}
