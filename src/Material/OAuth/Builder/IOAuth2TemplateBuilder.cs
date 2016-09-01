using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
namespace Material.Infrastructure.OAuth.Builder
{
    public interface IOAuth2TemplateBuilder
    {
        TCredentials BuildCredentials<TResourceProvider, TCredentials>()
            where TCredentials : TokenCredentials
            where TResourceProvider : ResourceProvider;

        IOAuthFacade<OAuth2Credentials> BuildFacade(
            OAuth2ResourceProvider resourceProvider,
            IOAuth2Authentication authentication,
            string clientId,
            string callbackUrl,
            AuthenticationInterfaceEnum ui);

        IOAuthAuthenticationTemplate<OAuth2Credentials> BuildTemplate<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId,
            string clientSecret)
            where TResourceProvider : ResourceProvider;
    }
}
