using Microservices.Web.Models;

namespace Microservices.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto> RegisterAsync(RegistrationRequestDto registerRequestDto);
        Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registerRequestDto);
    }
}
