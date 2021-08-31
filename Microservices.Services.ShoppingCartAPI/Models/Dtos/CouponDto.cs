
namespace Microservices.Services.ShoppingCartAPI.Models.Dtos;
public class CouponDto
{
    public Guid CouponId { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
}
