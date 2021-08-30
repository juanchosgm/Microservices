
using AutoMapper;

namespace Microservices.Services.OrderAPI;
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
