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

        public Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = Utility.SD.ApiType.GET,
                Url = Utility.SD.CouponApiBase + api + "/CouponApi"
            });
        }

        public Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            throw new NotImplementedException();
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
