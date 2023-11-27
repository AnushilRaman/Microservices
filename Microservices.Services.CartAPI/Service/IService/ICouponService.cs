using Microservices.Services.CartAPI.Models.Dto;

namespace Microservices.Services.CartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupons(string couponCode);
    }
}
