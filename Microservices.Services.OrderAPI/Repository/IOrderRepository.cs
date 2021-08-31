
using Microservices.Services.OrderAPI.Models;

namespace Microservices.Services.OrderAPI.Repository;
public interface IOrderRepository
{
    ValueTask<bool> AddOrderAsync(OrderHeader orderHeader);
    ValueTask UpdateOrderPaymentStatus(Guid orderHeaderId, bool paid);
}
