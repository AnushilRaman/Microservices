using Microservices.Services.CartAPI.Models.Dto;
using Microservices.Services.CartAPI.Service.IService;
using Newtonsoft.Json;

namespace Microservices.Services.CartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCoupons(string couponCode)
        {
            var client = httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/CouponApi/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
