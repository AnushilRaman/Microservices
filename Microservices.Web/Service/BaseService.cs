using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient httpClient = httpClientFactory.CreateClient("MicroserviceApi");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? apiResponse = null;
                switch (requestDto.apiType)
                {
                    case Utility.SD.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case Utility.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Utility.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Delete;
                        break;
                }
                apiResponse = await httpClient.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                return new() { IsSuccess = false, Message = ex.Message.ToString() };
            }
        }

    }
}
