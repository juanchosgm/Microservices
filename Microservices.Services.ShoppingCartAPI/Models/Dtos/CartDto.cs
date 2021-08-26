
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Microservices.Services.ShoppingCartAPI.Models.Dtos;

[ValidateNever]
public class CartDto
{
    public CartHeaderDto CartHeader { get; set; }
    public IEnumerable<CartDetailDto> CartDetails { get; set; }
}
