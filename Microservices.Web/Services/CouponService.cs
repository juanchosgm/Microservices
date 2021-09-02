
using Microservices.Web.Models;
using Microservices.Web.Services.IServices;

namespace Microservices.Web.Services;
public class CouponService : BaseService, ICouponService
{
    public CouponService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        : base(httpClientFactory, httpContextAccessor)
    {
    }

    public async Task<T> GetCouponAsync<T>(string couponCode)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.GET,
            Url = $"{SD.APIs}/api/coupon/{couponCode}",
            AccessToken = AccessToken
        });
    }
}
