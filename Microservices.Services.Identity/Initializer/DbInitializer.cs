
using IdentityModel;
using Microservices.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Microservices.Services.Identity.Initializer;
public class DbInitializer : IDbInitializer
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public void Initialize()
    {
        if (roleManager.FindByNameAsync(SD.Admin).Result == null)
        {
            roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
        }
        else
        {
            return;
        }
        ApplicationUser? adminUser = new()
        {
            UserName = "admin1@gmail.com",
            Email = "admin1@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "1111111",
            FirstName = "Ben",
            LastName = "Admin"
        };
        userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
        userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
        userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, $"{adminUser.FirstName} {adminUser.LastName}"),
            new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
            new Claim(JwtClaimTypes.Role, SD.Admin)
        }).GetAwaiter().GetResult();
        ApplicationUser? customerUser = new()
        {
            UserName = "customer1@gmail.com",
            Email = "customer1@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "1111111",
            FirstName = "Ben",
            LastName = "Cust"
        };
        userManager.CreateAsync(customerUser, "Customer123*").GetAwaiter().GetResult();
        userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
        userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, $"{customerUser.FirstName} {customerUser.LastName}"),
            new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
            new Claim(JwtClaimTypes.Role, SD.Customer)
        }).GetAwaiter().GetResult();
    }
}
