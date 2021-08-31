
using Microservices.Services.ShoppingCartAPI.Models.Dtos;
using Newtonsoft.Json;

namespace Microservices.Services.ShoppingCartAPI.Repository;
public class CouponRepository : ICouponRepository
{
    private readonly HttpClient httpClient;

    public CouponRepository(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CouponDto> GetCouponAsync(string couponCode)
    {
        HttpResponseMessage? request = await httpClient.GetAsync($"/api/coupon/{couponCode}");
        request.EnsureSuccessStatusCode();
        string? response = await request.Content.ReadAsStringAsync();
        ResponseDto? content = JsonConvert.DeserializeObject<ResponseDto>(response);
        if (content.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(content.Result));
        }
        return new();
    }
}
