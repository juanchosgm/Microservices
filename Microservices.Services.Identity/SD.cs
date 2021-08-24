
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Microservices.Services.Identity;
public static class SD
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static IEnumerable<IdentityResource> Resources => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<ApiScope> Scopes => new List<ApiScope>
    {
        new ApiScope("Microservices", "Microservices Server"),
        new ApiScope("read", "Read your data."),
        new ApiScope("write", "Write your data"),
        new ApiScope("delete", "Delete your data.")
    };

    public static IEnumerable<Client> Clients => new List<Client>
    {
        new Client
        {
            ClientId = "client",
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "read", "write", "profile" }
        },
        new Client
        {
            ClientId = "microservices.web",
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:44344/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:44344/signout-callback-oidc" },
            AllowedScopes = 
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "Microservices"
            }
        },
    };
}
