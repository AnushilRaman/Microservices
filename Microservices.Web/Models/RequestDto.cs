using static Microservices.Web.Utility.SD;

namespace Microservices.Web.Models
{
    public class RequestDto
    {
        public ApiType apiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object? Data { get; set; }
        public string AccessToken { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
