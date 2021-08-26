
using AutoMapper;
using Microservices.Services.ShoppingCartAPI.Models;
using Microservices.Services.ShoppingCartAPI.Models.Dtos;

namespace Microservices.Services.ShoppingCartAPI;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration? mappingConfig = new(config =>
        {
            config.CreateMap<ProductDto, Product>().ReverseMap();
            config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
            config.CreateMap<CartDetailDto, CartDetail>().ReverseMap();
            config.CreateMap<CartDto, Cart>().ReverseMap();
        });
        return mappingConfig;
    }
}
