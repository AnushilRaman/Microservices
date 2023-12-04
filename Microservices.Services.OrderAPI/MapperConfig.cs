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
               
            });
            return mappingConfig;
        }
    }
}
