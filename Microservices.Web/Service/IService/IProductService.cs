using Microservices.Web.Models;

namespace Microservices.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(string name);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto ProductDto);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto ProductDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
