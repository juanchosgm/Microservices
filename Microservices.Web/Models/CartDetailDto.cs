
namespace Microservices.Web.Models;
public class CartDetailDto
{
    public Guid CartDetailId { get; set; }
    public Guid CartHeaderId { get; set; }
    public virtual CartHeaderDto CartHeader { get; set; }
    public Guid ProductId { get; set; }
    public virtual ProductDto Product { get; set; }
    public int Count { get; set; }
}
