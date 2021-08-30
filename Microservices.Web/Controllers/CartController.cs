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
    private readonly ICouponService couponService;

    public CartController(IProductService productService, ICartService cartService,
        ICouponService couponService)
    {
        this.productService = productService;
        this.cartService = cartService;
        this.couponService = couponService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        CartDto? cart = await LoadCartByUser();
        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ApplyCoupon(CartDto cart)
    {
        ResponseDto? response = await cartService.ApplyCouponAsync<ResponseDto>(cart);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Remove(Guid cartDetailId)
    {
        ResponseDto? response = await cartService.RemoveFromCartAsync<ResponseDto>(cartDetailId);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveCoupon(CartDto cart)
    {
        ResponseDto? response = await cartService.RemoveCouponAsync<ResponseDto>(cart.CartHeader.UserId);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        CartDto? cart = await LoadCartByUser();
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CartDto cart)
    {
        try
        {
            ResponseDto? response = await cartService.CheckoutAsync<ResponseDto>(cart.CartHeader);
            return RedirectToAction(nameof(Confirmation));
        }
        catch (Exception)
        {
            return View(cart);
        }
    }

    [HttpGet]
    public IActionResult Confirmation()
    {
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
            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                ResponseDto? couponResponse = await couponService.GetCouponAsync<ResponseDto>(cart.CartHeader.CouponCode);
                if (couponResponse is not null && couponResponse.IsSuccess)
                {
                    CouponDto? coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResponse.Result));
                    cart.CartHeader.DiscountTotal = coupon.DiscountAmount;
                }
            }
            foreach (CartDetailDto? cartDetail in cart.CartDetails)
            {
                cart.CartHeader.OrderTotal += cartDetail.Product.Price * cartDetail.Count;
            }
            cart.CartHeader.OrderTotal -= cart.CartHeader.DiscountTotal;
        }
        return cart;
    }
}
