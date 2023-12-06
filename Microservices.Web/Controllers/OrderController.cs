using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Microservices.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        public IActionResult OrderIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDto responseDto = orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if (responseDto != null && responseDto.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new {data = list });
        }
    }
}
