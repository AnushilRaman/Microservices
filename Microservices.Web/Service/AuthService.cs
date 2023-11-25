using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;

namespace Microservices.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registerRequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = registerRequestDto,
                Url = SD.AuthApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.AuthAPI + "/" + "AssignRole"
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.AuthAPI + "/" + "login"
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registerRequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                apiType = SD.ApiType.POST,
                Data = registerRequestDto,
                Url = SD.AuthApiBase + "/" + SD.ApiName.api + "/" + SD.ApiName.AuthAPI + "/" + "register"
            }, withBearer: false);
        }
    }
}
