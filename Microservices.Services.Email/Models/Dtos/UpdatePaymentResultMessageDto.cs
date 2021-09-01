
namespace Microservices.Services.EmailAPI.Models.Dtos;
public class UpdatePaymentResultMessageDto
{
    public Guid OrderId { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }
}
