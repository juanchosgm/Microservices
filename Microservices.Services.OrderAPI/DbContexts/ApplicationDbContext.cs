
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.OrderAPI.DbContexts;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
