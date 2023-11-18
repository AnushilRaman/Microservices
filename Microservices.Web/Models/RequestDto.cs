using static Microservices.Web.Utility.SD;

namespace Microservices.Web.Models
{
    public class RequestDto
    {
        public ApiType apiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public string Data { get; set; }
        public string AccessToken { get; set; }
    }
}
