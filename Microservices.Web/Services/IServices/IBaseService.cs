
using Microservices.Web.Models;

namespace Microservices.Web.Services.IServices;
public interface IBaseService : IDisposable
{
    public ResponseDto ResponseModel { get; set; }
    Task<T?> SendAsync<T>(ApiRequest request);
}
