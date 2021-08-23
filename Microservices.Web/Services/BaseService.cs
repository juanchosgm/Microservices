
using Microservices.Web.Models;
using Microservices.Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.Web.Services;
public class BaseService : IBaseService
{
    public BaseService(IHttpClientFactory httpClientFactory)
    {
        ResponseModel = new ResponseDto();
        HttpClient = httpClientFactory;
    }

    public ResponseDto ResponseModel { get; set; }
    public IHttpClientFactory HttpClient { get; set; }

    public async Task<T?> SendAsync<T>(ApiRequest request)
    {
        try
        {
            HttpClient? client = HttpClient.CreateClient("MicroservicesAPI");
            HttpRequestMessage? message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(request.Url);
            client.DefaultRequestHeaders.Clear();
            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }
            HttpResponseMessage? response = null;
            message.Method = request.ApiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
            response = await client.SendAsync(message);
            string content = await response.Content.ReadAsStringAsync();
            T? result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }
        catch (Exception ex)
        {
            ResponseDto? dto = new ResponseDto
            {
                DisplayMessage = "Error",
                ErrorMessages = { Convert.ToString(ex.Message) },
                IsSuccess = false
            };
            string? content = JsonConvert.SerializeObject(dto);
            T? result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
