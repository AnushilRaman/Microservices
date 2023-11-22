using Microservices.Services.AuthAPI.Models.Dto;
using Microservices.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        protected ResponseDto responseDto;

        public AuthAPIController(IAuthService authService)
        {
            this.authService = authService;
            responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                responseDto.IsSuccess = true;
                responseDto.Message = errorMessage;
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                responseDto.IsSuccess = true;
                responseDto.Message = "Username or Password is incorrect";
                return BadRequest(responseDto);
            }
            responseDto.Result = loginResponse;
            return Ok(responseDto);
        }
    }
}
