
using Microservices.Web.Services;
using Microservices.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();
Microservices.Web.SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
Microservices.Web.SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
Microservices.Web.SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", opt =>
    {
        opt.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
        opt.GetClaimsFromUserInfoEndpoint = true;
        opt.ClientId = "microservices.web";
        opt.ClientSecret = "secret";
        opt.ResponseType = "code";
        opt.ClaimActions.MapJsonKey("role", "role", "role");
        opt.ClaimActions.MapJsonKey("sub", "sub", "sub");
        opt.TokenValidationParameters.NameClaimType = "name";
        opt.TokenValidationParameters.RoleClaimType = "role";
        opt.Scope.Add("Microservices");
        opt.SaveTokens = true;
    });

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
