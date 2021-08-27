
using AutoMapper;
using Microservices.Services.ProductAPI.Models;
using Microservices.Services.ProductAPI.Models.Dtos;

namespace Microservices.Services.ProductAPI;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration? mappingConfig = new(config =>
        {
            config.CreateMap<ProductDto, Product>().ReverseMap();
        });
        return mappingConfig;
    }
}
