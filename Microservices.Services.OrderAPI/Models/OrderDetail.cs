
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Services.OrderAPI.Models;
public class OrderDetail
{
    [Key]
    public Guid OrderDetailId { get; set; }
    public Guid OrderHeaderId { get; set; }
    [ForeignKey(nameof(OrderHeaderId))]
    public virtual OrderHeader OrderHeader { get; set; }
    public Guid ProductId { get; set; }
    public int Count { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}
