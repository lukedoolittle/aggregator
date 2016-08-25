using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.OAuth
{
    //TODO: break this into atleast OAuth1 and OAuth2 builders
    public class OAuthBuilder
    {
        private readonly IOAuthAuthorizerUIFactory _oauthAuthorizerUIFactory;
        private readonly IClientCredentials _clientCredentials;
        private readonly IOAuthSecurityStrategy _strategy;

        public OAuthBuilder(
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

        public IOAuthFacade<OAuth1Credentials> BuildOAuth1Facade(
            OAuth1ResourceProvider resourceProvider, 
            IOAuth1Authentication authentication,
            string consumerKey, 
            string consumerSecret,
            string callbackUrl)
        {
            return new OAuth1AuthenticationFacade(
                resourceProvider,
                consumerKey,
                consumerSecret,
                callbackUrl,
                authentication,
                _strategy);
        }

        public IOAuthFacade<OAuth2Credentials> BuildOAuth2Facade(
            OAuth2ResourceProvider resourceProvider,
            IOAuth2Authentication authentication,
            string clientId,
            string clientSecret,
            string callbackUrl)
        {
            return new OAuth2AuthenticationFacade(
                resourceProvider,
                clientId,
                callbackUrl,
                authentication,
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

        public IOAuthAuthenticationTemplate<OAuth2Credentials> BuildOAuth2Template<TResourceProvider>(
            IOAuthFacade<OAuth2Credentials> authentication,
            AuthenticationInterfaceEnum ui,
            string userId,
            string clientSecret,
            ResponseTypeEnum flow)
            where TResourceProvider : ResourceProvider
        {
            if (flow == ResponseTypeEnum.Code)
            {
                var callbackHandler = new OAuth2QueryCallbackHandler(
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
            else
            {
                IOAuthCallbackHandler callbackHandler = null;
                if (ui == AuthenticationInterfaceEnum.Dedicated)
                {
                    callbackHandler = new OAuth2QueryCallbackHandler(
                        _strategy,
                        OAuth2ParameterEnum.State.EnumToString(),
                        userId);
                }
                else
                {
                    callbackHandler = new OAuth2FragmentCallbackHandler(
                        _strategy,
                        OAuth2ParameterEnum.State.EnumToString(),
                        userId);
                }

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
}
