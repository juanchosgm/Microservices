using Microservices.Services.ShoppingCartAPI.Models.Dtos;
using Microservices.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.ShoppingCartAPI.Controllers;
[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository cartRepository;
    protected ResponseDto response;

    public CartController(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
        response = new();
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ResponseDto>> GetCart([FromRoute] string userId)
    {
        try
        {
            CartDto? cart = await cartRepository.GetCartByUserIdAsync(userId);
            response.Result = cart;
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
    public async Task<ActionResult<ResponseDto>> AddCart([FromBody] CartDto cart)
    {
        try
        {
            CartDto? cartResult = await cartRepository.CreateUpdateCartAsync(cart);
            response.Result = cart;
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
    public async Task<ActionResult<ResponseDto>> UpdateCart([FromBody] CartDto cart)
    {
        try
        {
            CartDto? cartResult = await cartRepository.CreateUpdateCartAsync(cart);
            response.Result = cart;
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
    public async Task<ActionResult<ResponseDto>> RemoveCart([FromRoute] Guid id)
    {
        try
        {
            bool removed = await cartRepository.RemoveFromCartAsync(id);
            response.Result = removed;
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
