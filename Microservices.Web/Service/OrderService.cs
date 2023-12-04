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
    }
}
