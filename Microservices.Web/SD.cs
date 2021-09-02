
namespace Microservices.Web;
public static class SD
{
    public static string APIs { get; set; }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH
    }
}
