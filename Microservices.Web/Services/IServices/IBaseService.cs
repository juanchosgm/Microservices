
using Microservices.Web.Models;

namespace Microservices.Web.Services.IServices;
public interface IBaseService : IDisposable
{
    Task<T?> SendAsync<T>(ApiRequest request);
}
