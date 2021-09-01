using Microservices.MessageBus;
using Microservices.Services.ShoppingCartAPI.Models.Dtos;
using Microservices.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.ShoppingCartAPI.Controllers;
[Route("api/cart")]
[ApiController]
public class CartApiController : ControllerBase
{
    private readonly ICartRepository cartRepository;
    private readonly ICouponRepository couponRepository;
    private readonly IMessageBus messageBus;
    private readonly IConfiguration configuration;
    protected ResponseDto response;

    public CartApiController(ICartRepository cartRepository, ICouponRepository couponRepository,
        IMessageBus messageBus,
        IConfiguration configuration)
    {
        this.cartRepository = cartRepository;
        this.couponRepository = couponRepository;
        this.messageBus = messageBus;
        this.configuration = configuration;
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

    [HttpPatch]
    public async Task<ActionResult<ResponseDto>> ApplyCupon([FromBody] CartDto cart)
    {
        try
        {
            bool couponAdded = await cartRepository.ApplyCouponAsync(cart.CartHeader.UserId, cart.CartHeader.CouponCode);
            response.Result = couponAdded;
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

    [HttpDelete("RemoveCoupon/{userId}")]
    public async Task<ActionResult<ResponseDto>> RemoveCoupon([FromRoute] string userId)
    {
        try
        {
            bool couponRemoved = await cartRepository.RemoveCouponAsync(userId);
            response.Result = couponRemoved;
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

    [HttpPost("Checkout")]
    public async Task<ActionResult<ResponseDto>> Checkout([FromBody] CheckoutHeaderDto checkoutHeader)
    {
        try
        {
            CartDto? cart = await cartRepository.GetCartByUserIdAsync(checkoutHeader.UserId);
            if (cart is null)
            {
                return BadRequest();
            }
            if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
            {
                CouponDto? coupon = await couponRepository.GetCouponAsync(checkoutHeader.CouponCode);
                if (checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new() { "Coupon price has changed, please confirm" };
                    response.DisplayMessage = "Coupon price has changed, please confirm";
                    return response;
                }
            }
            checkoutHeader.CartDetails = cart.CartDetails;
            await messageBus.PublishMessage(checkoutHeader, configuration["CheckoutMessageTopic"]);
            await cartRepository.ClearCartAsync(checkoutHeader.UserId);
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
