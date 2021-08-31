
using Microservices.Services.OrderAPI.DbContexts;
using Microservices.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.OrderAPI.Repository;
public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<ApplicationDbContext> options;

    public OrderRepository(DbContextOptions<ApplicationDbContext> options)
    {
        this.options = options;
    }

    public async ValueTask<bool> AddOrderAsync(OrderHeader orderHeader)
    {
        try
        {
            await using ApplicationDbContext? context = new ApplicationDbContext(options);
            context.OrderHeaders.Add(orderHeader);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async ValueTask UpdateOrderPaymentStatus(Guid orderHeaderId, bool paid)
    {
        await using ApplicationDbContext? context = new ApplicationDbContext(options);
        OrderHeader? orderHeader = await context.OrderHeaders.FirstOrDefaultAsync(oh => oh.OrderHeaderId == orderHeaderId);
        if (orderHeader is not null)
        {
            orderHeader.PaymentStatus = paid;
            await context.SaveChangesAsync();
        }
    }
}
