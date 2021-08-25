using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microservices.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Microservices.Services.Identity.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory;

    public ProfileService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string? sub = context.Subject.GetSubjectId();
        ApplicationUser? user = await userManager.FindByIdAsync(sub);
        ClaimsPrincipal? userClaims = await userClaimsPrincipalFactory.CreateAsync(user);
        List<Claim>? claims = userClaims.Claims.ToList()
            .Where(c => context.RequestedClaimTypes.Contains(c.Type))
            .ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
        if (userManager.SupportsUserRole)
        {
            IList<string>? roles = await userManager.GetRolesAsync(user);
            foreach (string? roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                if (roleManager.SupportsRoleClaims)
                {
                    IdentityRole? role = await roleManager.FindByNameAsync(roleName);
                    if (role is not null)
                    {
                        claims.AddRange(await roleManager.GetClaimsAsync(role));
                    }
                }
            }
        }
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string? sub = context.Subject.GetSubjectId();
        ApplicationUser? user = await userManager.FindByIdAsync(sub);
        context.IsActive = user is not null;
    }
}
