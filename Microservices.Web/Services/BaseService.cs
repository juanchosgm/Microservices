
using Microservices.Web.Models;
using Microservices.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Microservices.Web.Services;
public class BaseService : IBaseService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IHttpContextAccessor httpContextAccessor;

    public BaseService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        this.httpClientFactory = httpClientFactory;
        this.httpContextAccessor = httpContextAccessor;
    }

    protected string AccessToken => httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();

    public async Task<T?> SendAsync<T>(ApiRequest request)
    {
        try
        {
            HttpClient? client = httpClientFactory.CreateClient("MicroservicesAPI");
            HttpRequestMessage? message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(request.Url);
            client.DefaultRequestHeaders.Clear();
            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }
            if (!string.IsNullOrEmpty(request.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
            }
            HttpResponseMessage? response = null;
            message.Method = request.ApiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                SD.ApiType.PATCH => HttpMethod.Patch,
                _ => HttpMethod.Get,
            };
            response = await client.SendAsync(message);
            response = response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            T? result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }
        catch (Exception ex)
        {
            ResponseDto? dto = new()
            {
                DisplayMessage = "Error",
                ErrorMessages = new()
                {
                    Convert.ToString(ex.Message)
                },
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
