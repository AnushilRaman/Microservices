using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto responseDto = orderService.GetorderByid(orderId).GetAwaiter().GetResult();
            if (responseDto != null && responseDto.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
            }
            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDto);
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);
            if (response != null && response.IsSuccess)
            {
                TempData["successMessage"] = "Status Updated Successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            else
            {
                TempData["errorMessage"] = response?.Message;
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
        }

        [HttpPost("CompeleteOrder")]
        public async Task<IActionResult> CompeleteOrder(int orderId)
        {
            var response = await orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
            if (response != null && response.IsSuccess)
            {
                TempData["successMessage"] = "Status Updated Successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            else
            {
                TempData["errorMessage"] = response?.Message;
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
            if (response != null && response.IsSuccess)
            {
                TempData["successMessage"] = "Status Updated Successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            else
            {
                TempData["errorMessage"] = response?.Message;
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
        }


        [HttpGet]
        public IActionResult GetAll(string status)
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
                switch (status)
                {
                    case "approved":
                        list = list.Where(x => x.Status.ToLower() == status.ToLower());
                        break;
                    case "readyforpickup":
                        list = list.Where(x => x.Status.ToLower() == status.ToLower());
                        break;
                    case "cancelled":
                        list = list.Where(x => x.Status.ToLower() == status.ToLower());
                        break;
                    default:
                        break;
                }
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new { data = list });
        }
    }
}
