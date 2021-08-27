
using Microservices.Web.Models;
using Microservices.Web.Services.IServices;

namespace Microservices.Web.Services;
public class CartService : BaseService, ICartService
{
    public CartService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        : base(httpClientFactory, httpContextAccessor)
    {
    }

    public async Task<T> AddToCartAsync<T>(CartDto cart)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.POST,
            Data = cart,
            Url = $"{SD.ShoppingCartAPIBase}/api/cart",
            AccessToken = AccessToken
        });
    }

    public async Task<T> ApplyCouponAsync<T>(CartDto cart)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.PATCH,
            Data = cart,
            Url = $"{SD.ShoppingCartAPIBase}/api/cart",
            AccessToken = AccessToken
        });
    }

    public async Task<T> GetCartByUserIdAsync<T>(string userId)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.GET,
            Url = $"{SD.ShoppingCartAPIBase}/api/cart/{userId}",
            AccessToken = AccessToken
        });
    }

    public async Task<T> RemoveCouponAsync<T>(string userId)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{SD.ShoppingCartAPIBase}/api/cart/RemoveCoupon/{userId}",
            AccessToken = AccessToken
        });
    }

    public async Task<T> RemoveFromCartAsync<T>(Guid cartId)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{SD.ShoppingCartAPIBase}/api/cart/{cartId}",
            AccessToken = AccessToken
        });
    }

    public async Task<T> UpdateCartAsyncc<T>(CartDto cart)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.PUT,
            Data = cart,
            Url = $"{SD.ShoppingCartAPIBase}/api/cart",
            AccessToken = AccessToken
        });
    }
}
