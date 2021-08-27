
using AutoMapper;
using Microservices.Services.CouponAPI.DbContexts;
using Microservices.Services.CouponAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.CouponAPI.Repository;
public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public CouponRepository(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<CouponDto> GetCouponByCode(string couponCode)
    {
        var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
        return mapper.Map<CouponDto>(coupon);
    }
}
