using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Microservices.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void ClearToken()
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(SD.TokenCookies);
        }

        public string? GetToken()
        {
            string? token = string.Empty;
            bool? hasToken = httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.TokenCookies, out token);

            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(SD.TokenCookies, token);
        }
    }
}
