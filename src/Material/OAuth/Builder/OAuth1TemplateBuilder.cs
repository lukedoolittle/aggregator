using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Template;

namespace Material.Infrastructure.OAuth.Builder
{
    public class OAuth1TemplateBuilder
    {
        private readonly IOAuthAuthorizerUIFactory _oauthAuthorizerUIFactory;
        private readonly IClientCredentials _clientCredentials;
        private readonly IOAuthSecurityStrategy _strategy;

        public OAuth1TemplateBuilder(
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

        public IOAuthFacade<OAuth1Credentials> BuildFacade(
            OAuth1ResourceProvider resourceProvider,
            IOAuth1Authentication authentication,
            string consumerKey,
            string consumerSecret,
            string callbackUrl)
        {
            var handler = new OAuth1CallbackHandler(
                _strategy,
                OAuth1ParameterEnum.OAuthToken.EnumToString());

            return new OAuth1AuthenticationFacade(
                resourceProvider,
                consumerKey,
                consumerSecret,
                callbackUrl,
                authentication,
                _strategy,
                handler);
        }

        public IOAuthAuthenticationTemplate<OAuth1Credentials> BuildTemplate<TResourceProvider>(
            IOAuthFacade<OAuth1Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId,
            Uri callbackUri)
            where TResourceProvider : ResourceProvider
        {
            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider, OAuth1Credentials>(
                    ui,
                    authentication,
                    callbackUri);

            return new OAuth1AuthenticationTemplate(
                authenticationUI,
                authentication,
                _strategy,
                userId);
        }
    }
}
