using Microservices.MessageBus;
using Microservices.Services.AuthAPI.Models.Dto;
using Microservices.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.AuthAPI.Controllers
{
    [Route("api/AuthAPI")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMessageBus messageBus;
        protected ResponseDto responseDto;

        public AuthAPIController(IAuthService authService, IMessageBus messageBus)
        {
            this.authService = authService;
            this.messageBus = messageBus;
            responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {

            var errorMessage = await authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                responseDto.IsSuccess = false;
                responseDto.Message = errorMessage;
                return BadRequest(responseDto);
            }
            await messageBus.PublishMessage(model.Email, StaticClass.EmailregisterUserCart);
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Username or Password is incorrect";
                return BadRequest(responseDto);
            }
            responseDto.Result = loginResponse;
            return Ok(responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto loginRequestDto)
        {
            var loginResponse = await authService.AssignRole(loginRequestDto.Email,loginRequestDto.Role.ToUpper());
            if (!loginResponse)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Username or Password is incorrect";
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }
    }
}
