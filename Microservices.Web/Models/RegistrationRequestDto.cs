namespace Microservices.Web.Models
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumer { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? RoleName { get; set; }
    }
}
