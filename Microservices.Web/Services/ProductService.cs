
using Microservices.Web.Models;
using Microservices.Web.Services.IServices;

namespace Microservices.Web.Services;
public class ProductService : BaseService, IProductService
{
    public ProductService(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    public async Task<T?> CreateProductAsync<T>(ProductDto product)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.POST,
            Data = product,
            Url = $"{SD.ProductAPIBase}/api/products",
            AccessToken = string.Empty
        });
    }

    public async Task<T?> DeleteProductAsync<T>(Guid id)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{SD.ProductAPIBase}/api/products/{id}",
            AccessToken = string.Empty
        });
    }

    public async Task<T?> GetAllProductsAsync<T>()
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.GET,
            Url = $"{SD.ProductAPIBase}/api/products",
            AccessToken = string.Empty
        });
    }

    public async Task<T?> GetProductByIdAsync<T>(Guid id)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.GET,
            Url = $"{SD.ProductAPIBase}/api/products/{id}",
            AccessToken = string.Empty
        });
    }

    public async Task<T?> UpdateProductAsync<T>(ProductDto product)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.PUT,
            Data = product,
            Url = $"{SD.ProductAPIBase}/api/products",
            AccessToken = string.Empty
        });
    }
}
