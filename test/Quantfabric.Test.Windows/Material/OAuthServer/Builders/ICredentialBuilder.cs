using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public interface ICredentialBuilder<TCredentials, TRequest>
        where TCredentials : TokenCredentials
    {
        TCredentials BuildCredentials(TRequest request);
    }
}
