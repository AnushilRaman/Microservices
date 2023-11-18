using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;
        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto responseDto = await couponService.GetAllCouponAsync();
            if (responseDto != null)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
            return View(list);
        }
    }
}
