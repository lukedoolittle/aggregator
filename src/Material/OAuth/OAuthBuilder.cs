using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuthBuilder
    {
        private readonly IOAuthAuthorizerUIFactory _oauthAuthorizerUIFactory;
        private readonly IClientCredentials _clientCredentials;
        private readonly IOAuthFactory _oauthFactory;
        private readonly IOAuthSecurityStrategy _strategy;

        public OAuthBuilder(
            IOAuthAuthorizerUIFactory oauthAuthorizerUIFactory, 
            IClientCredentials clientCredentials, 
            IOAuthFactory oauthFactory, 
            IOAuthSecurityStrategy strategy = null)
        {
            _oauthAuthorizerUIFactory = oauthAuthorizerUIFactory;
            _clientCredentials = clientCredentials;
            _oauthFactory = oauthFactory;
            _strategy = strategy;
        }

        public TCredentials BuildCredentials<TResourceProvider, TCredentials>()
            where TCredentials : TokenCredentials
            where TResourceProvider : ResourceProvider
        {
            return _clientCredentials
                .GetClientCredentials<TResourceProvider, TCredentials>();
        }

        public IOAuthFacade<OAuth1Credentials> BuildOAuth1Facade(
            OAuth1ResourceProvider resourceProvider, 
            string consumerKey, 
            string consumerSecret,
            string callbackUrl)
        {
            return new OAuth1AuthenticationFacade(
                resourceProvider,
                consumerKey,
                consumerSecret,
                callbackUrl,
                _oauthFactory.GetOAuth1());
        }

        public IOAuthFacade<OAuth2Credentials> BuildOAuth2Facade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string clientSecret,
            string userId,
            string callbackUrl)
        {
            return new OAuth2AuthenticationFacade(
                resourceProvider,
                clientId,
                clientSecret,
                userId,
                callbackUrl,
                _oauthFactory.GetOAuth2(),
                _strategy);
        }

        public OAuthAuthenticationTemplate<OAuth1Credentials> BuildOAuth1Template<TResourceProvider>(
            IOAuthFacade<OAuth1Credentials> authentication,
            AuthenticationInterfaceEnum ui)
            where TResourceProvider : ResourceProvider
        {
            var handler = new OAuthCallbackHandler();
            
            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider>(ui, handler);

            return new OAuthAuthenticationTemplate<OAuth1Credentials>(
                authenticationUI,
                authentication);
        }

        public OAuthAuthenticationTemplate<OAuth2Credentials> BuildOAuth2CodeTemplate<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId)
            where TResourceProvider : ResourceProvider
        {
            var callbackHandler = new OAuth2CallbackHandler(userId, _strategy);

            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider>(ui, callbackHandler);
            
            return new OAuthAuthenticationTemplate<OAuth2Credentials>(
                authenticationUI,
                authentication);
        }

        public OAuthAuthenticationTemplate<OAuth2Credentials> BuildOAuth2TokenTemplate<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId)
            where TResourceProvider : ResourceProvider
        {
            var callbackHandler = new OAuth2CallbackHandler(userId, _strategy);

            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider>(ui, callbackHandler);

            return new OAuthTokenAuthenticationTemplate<OAuth2Credentials>(
                authenticationUI,
                authentication);
        }
    }
}
