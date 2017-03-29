using System.Threading.Tasks;
using Material.Domain.Core;
using Material.HttpClient;

namespace Material.Contracts
{
    public interface IProtectedResourceAdapter
    {
        Task<TResponse> ForProtectedResource<TRequest, TResponse>(
            TRequest request)
            where TRequest : OAuthRequest;

        Task<HttpResponse> ForProtectedResource<TRequest>(
            TRequest request)
            where TRequest : OAuthRequest;
    }
}
