using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using System;
using System.Reflection.Metadata;

namespace Microservices.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;

        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponApiBase + SD.ApiName.api + SD.ApiName.CouponApi
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.DELETE,
                Url = SD.CouponApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CouponApi + "/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CouponApi
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.CouponApiBase +"/" +SD.ApiName.api + "/" + SD.ApiName.CouponApi + "/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CouponApi + "/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CouponApi
            });
        }
    }
}
