using Microservices.Web.Models;
using Microservices.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers;
public class CartController : Controller
{
    private readonly IProductService productService;
    private readonly ICartService cartService;

    public CartController(IProductService productService, ICartService cartService)
    {
        this.productService = productService;
        this.cartService = cartService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        var cart = await LoadCartByUser();
        return View(cart);
    }

    public async Task<IActionResult> Remove(Guid cartDetailId)
    {
        ResponseDto? response = await cartService.RemoveFromCartAsync<ResponseDto>(cartDetailId);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    private async Task<CartDto> LoadCartByUser()
    {
        string? userId = User.FindFirst("sub").Value;
        ResponseDto? response = await cartService.GetCartByUserIdAsync<ResponseDto>(userId);
        CartDto cart = new();
        if (response is not null && response.IsSuccess)
        {
            cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
        }
        if (cart.CartHeader is not null)
        {
            foreach (CartDetailDto? cartDetail in cart.CartDetails)
            {
                cart.CartHeader.OrderTotal += cartDetail.Product.Price * cartDetail.Count;
            }
        }
        return cart;
    }
}
