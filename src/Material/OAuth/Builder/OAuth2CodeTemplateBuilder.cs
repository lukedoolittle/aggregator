using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Template;

namespace Material.Infrastructure.OAuth.Builder
{
    public class OAuth2CodeTemplateBuilder : IOAuth2TemplateBuilder
    {
        private readonly IOAuthAuthorizerUIFactory _oauthAuthorizerUIFactory;
        private readonly IClientCredentials _clientCredentials;
        private readonly IOAuthSecurityStrategy _strategy;

        public OAuth2CodeTemplateBuilder(
            IOAuthAuthorizerUIFactory oauthAuthorizerUIFactory,
            IClientCredentials clientCredentials,
            IOAuthSecurityStrategy strategy)
        {
            _oauthAuthorizerUIFactory = oauthAuthorizerUIFactory;
            _clientCredentials = clientCredentials;
            _strategy = strategy;
        }

        public TCredentials BuildCredentials<TResourceProvider, TCredentials>()
            where TCredentials : TokenCredentials
            where TResourceProvider : ResourceProvider
        {
            return _clientCredentials
                .GetClientCredentials<TResourceProvider, TCredentials>();
        }

        public IOAuthFacade<OAuth2Credentials> BuildFacade(
            OAuth2ResourceProvider resourceProvider,
            IOAuth2Authentication authentication,
            string clientId,
            string callbackUrl,
            AuthenticationInterfaceEnum ui)
        {
            var handler = new OAuth2QueryCallbackHandler(
                _strategy,
                OAuth2ParameterEnum.State.EnumToString());

            return new OAuth2AuthenticationFacade(
                resourceProvider,
                clientId,
                callbackUrl,
                authentication,
                _strategy,
                handler);
        }

        public IOAuthAuthenticationTemplate<OAuth2Credentials> BuildTemplate<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId,
            string clientSecret)
            where TResourceProvider : ResourceProvider
        {
            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    ui,
                    authentication);

            return new OAuth2CodeAuthenticationTemplate(
                    authenticationUI,
                    authentication,
                    clientSecret);
        }
    }
}
