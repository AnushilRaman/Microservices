using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;

namespace Microservices.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService baseService;

        public OrderService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.OrderApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.OrderApi + "/CreateOrder"
            });
        }

        public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.OrderApi + "/CreateStripeSession"
            });
        }

        public async Task<ResponseDto?> GetAllOrder(string? userId)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.OrderApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.OrderApi + "/GetOrders/"+ userId
            });
        }

        public async Task<ResponseDto?> GetorderByid(int orderId)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.GET,
                Url = SD.OrderApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.OrderApi + "/GetOrder/" + orderId
            });
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = newStatus,
                Url = SD.OrderApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.OrderApi + "/UpdateOrderStatus/"+ orderId
            });
        }

        public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrderApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.OrderApi + "/ValidateStripeSession"
            });
        }
    }
}
