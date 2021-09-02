
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
            Url = $"{SD.APIs}/api/cart",
            AccessToken = AccessToken
        });
    }

    public async Task<T> ApplyCouponAsync<T>(CartDto cart)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.PATCH,
            Data = cart,
            Url = $"{SD.APIs}/api/cart",
            AccessToken = AccessToken
        });
    }

    public async Task<T> CheckoutAsync<T>(CartHeaderDto cartHeader)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.POST,
            Data = cartHeader,
            Url = $"{SD.APIs}/api/cart/Checkout",
            AccessToken = AccessToken
        });
    }

    public async Task<T> GetCartByUserIdAsync<T>(string userId)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.GET,
            Url = $"{SD.APIs}/api/cart/{userId}",
            AccessToken = AccessToken
        });
    }

    public async Task<T> RemoveCouponAsync<T>(string userId)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{SD.APIs}/api/cart/RemoveCoupon/{userId}",
            AccessToken = AccessToken
        });
    }

    public async Task<T> RemoveFromCartAsync<T>(Guid cartId)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{SD.APIs}/api/cart/{cartId}",
            AccessToken = AccessToken
        });
    }

    public async Task<T> UpdateCartAsyncc<T>(CartDto cart)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.PUT,
            Data = cart,
            Url = $"{SD.APIs}/api/cart",
            AccessToken = AccessToken
        });
    }
}
