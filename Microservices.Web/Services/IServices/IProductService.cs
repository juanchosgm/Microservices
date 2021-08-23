
using Microservices.Web.Models;

namespace Microservices.Web.Services.IServices;
public interface IProductService : IBaseService
{
    Task<T?> GetAllProductsAsync<T>();
    Task<T?> GetProductByIdAsync<T>(Guid id);
    Task<T?> CreateProductAsync<T>(ProductDto product);
    Task<T?> UpdateProductAsync<T>(ProductDto product);
    Task<T?> DeleteProductAsync<T>(Guid id);
}
