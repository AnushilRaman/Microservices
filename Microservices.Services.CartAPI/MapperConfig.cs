using AutoMapper;
using Microservices.Services.CartAPI.Models;
using Microservices.Services.CartAPI.Models.Dto;


namespace Microservices.Services.CartAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
                config.CreateMap<CartHeaderDto, CartHeaders>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
