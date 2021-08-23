
using AutoMapper;
using Microservices.ProductAPI.Models;
using Microservices.ProductAPI.Models.Dtos;

namespace Microservices.ProductAPI;
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
