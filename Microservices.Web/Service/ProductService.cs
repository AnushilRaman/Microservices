using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using System.Xml.Linq;

namespace Microservices.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;

        public ProductService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto ProductDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = ProductDto,
                Url = SD.ProductApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.ProductApi,
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.DELETE,
                Url = SD.ProductApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.ProductApi + "/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.ProductApi
            });
        }

        public async Task<ResponseDto?> GetProductAsync(string name)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.ProductApi + "/" + name
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.ProductApi + "/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductsAsync(ProductDto ProductDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.PUT,
                Data = ProductDto,
                Url = SD.ProductApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.ProductApi,
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
