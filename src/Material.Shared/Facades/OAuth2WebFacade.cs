using System;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.OAuth;

namespace Material.Facades
{
    public class OAuth2WebFacade<TResourceProvider> : OAuth2AuthenticationFacade
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        public OAuth2WebFacade(
            string clientId, 
            string clientSecret, 
            string userId, 
            string callbackUri, 
            IOAuthSecurityStrategy strategy) :
                base(
                    new TResourceProvider(), 
                    clientId, 
                    clientSecret, 
                    userId, 
                    callbackUri, 
                    new OAuth2Authentication(), 
                    strategy)
        { }

        public OAuth2WebFacade(
            string clientId,
            string clientSecret,
            string userId,
            string callbackUri) :
                base(
                    new TResourceProvider(),
                    clientId,
                    clientSecret,
                    userId,
                    callbackUri,
                    new OAuth2Authentication(),
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(), 
                        TimeSpan.FromMinutes(2)))
        { }

        public OAuth2WebFacade(
            string clientId,
            string userId,
            string callbackUri) :
                this(
                    clientId, 
                    null, 
                    userId, 
                    callbackUri)
        { }

        public OAuth2WebFacade(
            string clientId,
            string userId,
            string callbackUri,
            IOAuthSecurityStrategy strategy) :
                this(
                    clientId,
                    null, 
                    userId, 
                    callbackUri, 
                    strategy)
        { }

        public OAuth2Credentials ParseAndValidateCallback(
            Uri responseUri)
        {
            var handler = new OAuth2CallbackHandler(
                _userId, 
                _strategy);

            return handler.ParseAndValidateCallback<OAuth2Credentials>(
                responseUri);
        }
    }
}
