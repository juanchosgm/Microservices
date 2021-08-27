
using Microservices.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.CouponAPI.DbContexts;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = Guid.NewGuid(),
            CouponCode = "10OFF",
            DiscountAmount = 10d
        });
        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = Guid.NewGuid(),
            CouponCode = "20OFF",
            DiscountAmount = 20d
        });
    }
}
