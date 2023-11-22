using Microservices.Services.AuthAPI.Models;
using Microservices.Services.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Microservices.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            this._jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser applicationUser)
        {
            //designed for creating and validating Json Web Tokens
            var tokenHandler = new JwtSecurityTokenHandler();
            //Encoding the Secret key
            var Key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            //Claims represent attributes of the subject that are useful in the context of authentication and authorization operations by using the users unique information
            var claims = new List<Claim>()
            {
              new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email),
              new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id),
              new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
            };

            //Contains User related information and some properties which used to create a security token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
