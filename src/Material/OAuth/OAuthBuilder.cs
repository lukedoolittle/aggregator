using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

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
            IOAuthSecurityStrategy strategy)
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
                _oauthFactory.GetOAuth1(),
                _strategy);
        }

        public IOAuthFacade<OAuth2Credentials> BuildOAuth2Facade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string clientSecret,
            string callbackUrl)
        {
            return new OAuth2AuthenticationFacade(
                resourceProvider,
                clientId,
                callbackUrl,
                _oauthFactory.GetOAuth2(),
                _strategy);
        }

        public IOAuthAuthenticationTemplate<OAuth1Credentials> BuildOAuth1Template<TResourceProvider>(
            IOAuthFacade<OAuth1Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId)
            where TResourceProvider : ResourceProvider
        {
            var handler = new OAuth1CallbackHandler(
                _strategy,
                OAuth1ParameterEnum.OAuthToken.EnumToString(),
                userId);
            
            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider>(ui, handler);

            return new OAuth1AuthenticationTemplate(
                authenticationUI,
                authentication, 
                _strategy, 
                userId);
        }

        public IOAuthAuthenticationTemplate<OAuth2Credentials> BuildOAuth2CodeTemplate<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId,
            string clientSecret)
            where TResourceProvider : ResourceProvider
        {
            var callbackHandler = new OAuth2CallbackHandler(
                _strategy, 
                OAuth2ParameterEnum.State.EnumToString(), 
                userId);

            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider>(
                    ui, 
                    callbackHandler);
            
            return new OAuth2CodeAuthenticationTemplate(
                authenticationUI,
                authentication,
                clientSecret);
        }

        public IOAuthAuthenticationTemplate<OAuth2Credentials> BuildOAuth2TokenTemplate<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId)
            where TResourceProvider : ResourceProvider
        {
            var callbackHandler = new OAuth2TokenCallbackHandler(
                _strategy,
                OAuth2ParameterEnum.State.EnumToString(),
                userId);

            var authenticationUI = _oauthAuthorizerUIFactory
                .GetAuthorizer<TResourceProvider>(
                    ui, 
                    callbackHandler);

            return new OAuth2TokenAuthenticationTemplate(
                authenticationUI,
                authentication);
        }
    }
}
