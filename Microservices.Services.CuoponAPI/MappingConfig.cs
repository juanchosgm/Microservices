
using AutoMapper;
using Microservices.Services.CouponAPI.Models;
using Microservices.Services.CouponAPI.Models.Dtos;

namespace Microservices.Services.CouponAPI;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration? mappingConfig = new(config =>
        {
            config.CreateMap<CouponDto, Coupon>().ReverseMap();
        });
        return mappingConfig;
    }
}
