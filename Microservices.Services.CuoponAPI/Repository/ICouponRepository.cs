
using Microservices.Services.CouponAPI.Models.Dtos;

namespace Microservices.Services.CouponAPI.Repository;
public interface ICouponRepository
{
    Task<CouponDto> GetCouponByCode(string couponCode);
}
