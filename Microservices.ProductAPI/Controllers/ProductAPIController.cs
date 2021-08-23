using Microservices.ProductAPI.Models.Dtos;
using Microservices.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ProductAPI.Controllers;
[Route("api/products")]
[ApiController]
public class ProductAPIController : ControllerBase
{
    private readonly IProductRepository productRepository;
    protected ResponseDto response;

    public ProductAPIController(IProductRepository productRepository)
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
            response.ErrorMessages = new List<string>
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
            response.ErrorMessages = new List<string>
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpPost]
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
            response.ErrorMessages = new List<string>
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpPut]
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
            response.ErrorMessages = new List<string>
            {
                ex.ToString()
            };
        }
        return response;
    }

    [HttpDelete("{id}")]
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
            response.ErrorMessages = new List<string>
            {
                ex.ToString()
            };
        }
        return response;
    }
}
