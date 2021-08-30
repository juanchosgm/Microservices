﻿
using System.ComponentModel.DataAnnotations;

namespace Microservices.Services.CouponAPI.Models;
public class Coupon
{
    [Key]
    public Guid CouponId { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
}
