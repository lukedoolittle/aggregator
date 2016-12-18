using System.Threading.Tasks;
using Material.Infrastructure;

namespace Material.Contracts
{
    public interface IOAuthProtectedResourceAdapter
    {
        Task<TResponse> ForProtectedResource<TRequest, TResponse>(
            TRequest request)
            where TRequest : OAuthRequest;
    }
}
