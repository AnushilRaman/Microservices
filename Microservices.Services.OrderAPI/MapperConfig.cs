using AutoMapper;
using Microservices.Services.OrderAPI.Models;
using Microservices.Services.OrderAPI.Models.Dto;
using System.Reflection.PortableExecutable;

namespace Microservices.Services.OrderAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>().
                ForMember(dest => dest.CartTotal, x => x.MapFrom(src => src.CartTotal)).ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>().
                ForMember(dest => dest.ProductName, x => x.MapFrom(src => src.Product.Name)).
                ForMember(dest => dest.Price, x => x.MapFrom(src => src.Product.Price)).ReverseMap();

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();
                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
