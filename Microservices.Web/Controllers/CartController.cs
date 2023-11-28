using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedLoggedInUser());
        }

        private async Task<CartDto> LoadCartDtoBasedLoggedInUser()
        {
            var userId = User.Claims.Where(x => x.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            else
            {
                return new CartDto();
            }
        }
    }
}
