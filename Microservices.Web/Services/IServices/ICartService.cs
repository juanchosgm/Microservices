
using Microservices.Web.Models;

namespace Microservices.Web.Services.IServices;
public interface ICartService
{
    Task<T> GetCartByUserIdAsync<T>(string userId);
    Task<T> AddToCartAsync<T>(CartDto cart);
    Task<T> UpdateCartAsyncc<T>(CartDto cart);
    Task<T> RemoveFromCartAsync<T>(Guid cartId);
    Task<T> ApplyCouponAsync<T>(CartDto cart);
    Task<T> RemoveCouponAsync<T>(string userId);
    Task<T> CheckoutAsync<T>(CartHeaderDto cartHeader);
}
