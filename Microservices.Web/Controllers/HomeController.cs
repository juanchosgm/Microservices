using Microservices.Web.Models;
using Microservices.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Microservices.Web.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService productService;

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<ProductDto> products = new();
        ResponseDto? response = await productService.GetAllProductsAsync<ResponseDto>();
        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        }
        return View(products);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(Guid productId)
    {
        ProductDto? product = new();
        ResponseDto? response = await productService.GetProductByIdAsync<ResponseDto>(productId);
        if (response != null && response.IsSuccess)
        {
            product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
        }
        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        string? accessToken = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}
