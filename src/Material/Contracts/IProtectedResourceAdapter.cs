using System.Threading.Tasks;
using Foundations.HttpClient;
using Material.Infrastructure;

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
