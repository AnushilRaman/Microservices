using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using System;
using System.Reflection.Metadata;

namespace Microservices.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService baseService;

        public CartService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.CartApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CartApi + "/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.CartApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CartApi + "/GetCart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsid)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = cartDetailsid,
                Url = SD.CartApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CartApi + "/RemoveCoupon"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.CartApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.CartApi + "/CartUpsert"
            });
        }
    }
}
