using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IRefreshTokenFacade
    {
        Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials<TResourceProvider>(
            OAuth2Credentials expiredCredentials,
            TResourceProvider provider = null)
            where TResourceProvider : OAuth2ResourceProvider, new();
    }
}
