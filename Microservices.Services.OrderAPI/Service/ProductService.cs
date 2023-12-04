using Microservices.Services.OrderAPI.Models.Dto;
using Microservices.Services.OrderAPI.Service.IService;
using Newtonsoft.Json;

namespace Microservices.Services.OrderAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> Getproducts()
        {
            var client = httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/ProductApi");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDto>();
        }
    }
}
