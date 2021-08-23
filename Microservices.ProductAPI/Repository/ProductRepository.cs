
using AutoMapper;
using Microservices.ProductAPI.DbContexts;
using Microservices.ProductAPI.Models;
using Microservices.ProductAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.ProductAPI.Repository;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public ProductRepository(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ProductDto> CreateUpdateProductAsync(ProductDto product)
    {
        Product? productEntity = mapper.Map<ProductDto, Product>(product);
        if (productEntity.ProductId != Guid.Empty)
        {
            context.Products.Update(productEntity);
        }
        else
        {
            context.Products.Add(productEntity);
        }
        await context.SaveChangesAsync();
        return mapper.Map<Product, ProductDto>(productEntity);
    }

    public async ValueTask<bool> DeleteProductAsync(Guid productId)
    {
        try
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return false;
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid productId)
    {
        Product? product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        return mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        List<Product>? products = await context.Products.ToListAsync();
        return mapper.Map<List<ProductDto>>(products);
    }
}
