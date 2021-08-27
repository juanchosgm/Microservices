
using AutoMapper;

namespace Microservices.Services.CouponAPI;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration? mappingConfig = new(config =>
        {
        });
        return mappingConfig;
    }
}
