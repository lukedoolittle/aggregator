using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Callback;
using Material.Infrastructure.OAuth.Template;

namespace Material.Infrastructure.OAuth.Builder
{
    public class OAuth2TokenTemplateBuilder : IOAuth2TemplateBuilder
    {
        private readonly IOAuthAuthorizerUIFactory _oauthAuthorizerUIFactory;
        private readonly IClientCredentials _clientCredentials;
        private readonly IOAuthSecurityStrategy _strategy;

        public OAuth2TokenTemplateBuilder(
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
            IOAuthCallbackHandler<OAuth2Credentials> handler = null;
            if (ui == AuthenticationInterfaceEnum.Dedicated)
            {
                handler = new OAuth2QueryCallbackHandler(
                    _strategy,
                    OAuth2ParameterEnum.State.EnumToString());
            }
            else
            {
                handler = new OAuth2FragmentCallbackHandler(
                    _strategy,
                    OAuth2ParameterEnum.State.EnumToString());
            }

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
            string clientSecret,
            Uri callbackUri)
            where TResourceProvider : ResourceProvider
        {
            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    ui,
                    authentication,
                    callbackUri);

            return new OAuth2TokenAuthenticationTemplate(
                authenticationUI,
                authentication);
        }
    }
}
