
namespace Microservices.Web.Services.IServices;
public interface ICouponService
{
    Task<T> GetCouponAsync<T>(string couponCode);
}
