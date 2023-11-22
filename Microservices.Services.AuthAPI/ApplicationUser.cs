using Microsoft.AspNetCore.Identity;

namespace Microservices.Services.AuthAPI
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
