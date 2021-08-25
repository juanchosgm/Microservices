using Microservices.Web.Models;
using Microservices.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers;
public class ProductController : Controller
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto>? products = new();
        ResponseDto? response = await productService.GetAllProductsAsync<ResponseDto>();
        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        }
        return View(products);
    }

    [HttpGet]
    public IActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductCreate(ProductDto product)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await productService.CreateProductAsync<ResponseDto>(product);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> ProductEdit(Guid productId)
    {
        ResponseDto? response = await productService.GetProductByIdAsync<ResponseDto>(productId);
        if (response != null && response.IsSuccess)
        {
            ProductDto? product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(product);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductEdit(ProductDto product)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await productService.UpdateProductAsync<ResponseDto>(product);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(product);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductDelete(Guid productId)
    {
        ResponseDto? response = await productService.GetProductByIdAsync<ResponseDto>(productId);
        if (response != null && response.IsSuccess)
        {
            ProductDto? product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(product);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductDelete(ProductDto product)
    {
        if (ModelState.IsValid)
        {
            ResponseDto response = await productService.DeleteProductAsync<ResponseDto>(product.ProductId);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(product);
    }
}
