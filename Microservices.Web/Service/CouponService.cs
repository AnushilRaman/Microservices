using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using System.Reflection.Metadata;

namespace Microservices.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;
        private const string api = "/api";

        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = Utility.SD.ApiType.POST,
                Data = couponDto,
                Url = Utility.SD.CouponApiBase + api + "/CouponApi"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = Utility.SD.ApiType.DELETE,
                Url = Utility.SD.CouponApiBase + api + "/CouponApi" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = Utility.SD.ApiType.GET,
                Url = Utility.SD.CouponApiBase + api + "/CouponApi"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = Utility.SD.ApiType.GET,
                Url = Utility.SD.CouponApiBase + api + "/CouponApi" + couponCode
            });
        }

        public Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            throw new NotImplementedException();
        }
    }
}
