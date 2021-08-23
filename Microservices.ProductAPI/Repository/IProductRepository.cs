
using Microservices.ProductAPI.Models.Dtos;

namespace Microservices.ProductAPI.Repository;
public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<ProductDto> GetProductByIdAsync(Guid productId);
    Task<ProductDto> CreateUpdateProductAsync(ProductDto product);
    ValueTask<bool> DeleteProductAsync(Guid productId);
}
