using Microservices.Services.CouponAPI.Models.Dtos;
using Microservices.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.CouponAPI.Controllers;
[Route("api/coupon")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository couponRepository;
    protected ResponseDto response;

    public CouponController(ICouponRepository couponRepository)
    {
        this.couponRepository = couponRepository;
        response = new ResponseDto();
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<ResponseDto>> GetCoupon(string code)
    {
        try
        {
            CouponDto? coupon = await couponRepository.GetCouponByCode(code);
            response.Result = coupon;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new()
            {
                ex.ToString()
            };
        }
        return response;
    }
}
