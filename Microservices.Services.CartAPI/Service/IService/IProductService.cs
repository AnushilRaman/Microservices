using Microservices.Services.CartAPI.Models.Dto;

namespace Microservices.Services.CartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> Getproducts();
    }
}
