
namespace Microservices.Services.CouponAPI.Models;
public class Coupon
{
    public Guid CouponId { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
}
