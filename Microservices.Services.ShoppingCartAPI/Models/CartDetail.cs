
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Services.ShoppingCartAPI.Models;
public class CartDetail
{
    public Guid CartDetailId { get; set; }
    public Guid CartHeaderId { get; set; }
    [ForeignKey(nameof(CartHeaderId))]
    public virtual CartHeader CartHeader { get; set; }
    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product {  get; set; }
    public int Count { get; set; }
}
