using AutoMapper;
using Microservices.Services.CartAPI.Models;
using Microservices.Services.CartAPI.Models.Dto;
using System.Reflection.PortableExecutable;


namespace Microservices.Services.CartAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaders, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
