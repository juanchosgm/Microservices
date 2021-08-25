
using AutoMapper;
using Microservices.Services.ProductAPI.Models;
using Microservices.Services.ProductAPI.Models.Dtos;

namespace Microservices.Services.ProductAPI;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration? mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDto, Product>();
            config.CreateMap<Product, ProductDto>();
        });
        return mappingConfig;
    }
}
