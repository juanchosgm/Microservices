using Microservices.Services.ProductAPI.Models.Dtos;
using Microservices.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.ProductAPI.Controllers;
[Route("api/products")]
[ApiController]
public class ProductApiController : ControllerBase
{
    private readonly IProductRepository productRepository;
    protected ResponseDto response;

    public ProductApiController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
        response = new ResponseDto();
    }

    [HttpGet]   
    public async Task<ActionResult<ResponseDto>> Get()
    {
        try
        {
            IEnumerable<ProductDto>? products = await productRepository.GetProductsAsync();
            response.Result = products;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new()
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto>> Get([FromRoute] Guid id)
    {
        try
        {
            ProductDto? product = await productRepository.GetProductByIdAsync(id);
            response.Result = product;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new()
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> Post([FromBody] ProductDto product)
    {
        try
        {
            ProductDto? model = await productRepository.CreateUpdateProductAsync(product);
            response.Result = model;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new()
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> Put([FromBody] ProductDto product)
    {
        try
        {
            ProductDto? model = await productRepository.CreateUpdateProductAsync(product);
            response.Result = model;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new()
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDto>> Delete([FromRoute] Guid id)
    {
        try
        {
            var isSucess = await productRepository.DeleteProductAsync(id);
            response.Result = isSucess;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new()
            {
                ex.ToString()
            };
        }
        return response;
    }
}
