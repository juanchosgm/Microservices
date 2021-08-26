
using Microservices.Services.ShoppingCartAPI.Models.Dtos;

namespace Microservices.Services.ShoppingCartAPI.Repository;
public interface ICartRepository
{
    Task<CartDto> GetCartByUserIdAsync(string userId);
    Task<CartDto> CreateUpdateCartAsync(CartDto cart);
    ValueTask<bool> RemoveFromCartAsync(Guid cartDetailId);
    ValueTask<bool> ClearCartAsync(string userId);
}
