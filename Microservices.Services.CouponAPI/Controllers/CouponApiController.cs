using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            return Ok("Without paramter");
        }
        [HttpGet]
        [Route("Getstring")]
        public IActionResult Getstring(string message)
        {
            message = "With Paramter";
            return Ok(message);
        }
    }
}
