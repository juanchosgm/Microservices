﻿
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.CouponAPI.DbContexts;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}