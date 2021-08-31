
using Microservices.Services.ShoppingCartAPI.Models.Dtos;

namespace Microservices.Services.ShoppingCartAPI.Repository;
public interface ICouponRepository
{
    Task<CouponDto> GetCouponAsync(string couponCode);
}
