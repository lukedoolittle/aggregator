using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public enum CallbackType
    {
        Localhost,
        Protocol
    }

    public interface IClientCredentials
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TCredentials GetClientCredentials<TService, TCredentials>(
            CallbackType callbackType)
            where TService : ResourceProvider
            where TCredentials : TokenCredentials;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TCredentials GetClientCredentials<TService, TCredentials>()
            where TService : ResourceProvider
            where TCredentials : TokenCredentials;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        OAuth2Credentials GetJsonWebTokenCredentials<TService>()
            where TService : ResourceProvider;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        ApiKeyCredentials GetApiKeyCredentials<TService>()
            where TService : ApiKeyResourceProvider;
    }
}
