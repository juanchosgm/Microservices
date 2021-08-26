
using Microservices.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.ShoppingCartAPI.DbContexts;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
}
