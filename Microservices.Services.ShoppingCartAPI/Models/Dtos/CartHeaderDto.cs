namespace Microservices.Services.ShoppingCartAPI.Models.Dtos;
public class CartHeaderDto
{
    public Guid CartHeaderId { get; set; }
    public string UserId { get; set; }
    public string CouponCode { get; set; }
}
