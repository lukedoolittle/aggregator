using System;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.OAuth;

namespace Material.Facades
{
    public class OAuth1WebFacade<TResourceProvider> : OAuth1AuthenticationFacade
        where TResourceProvider: OAuth1ResourceProvider, new()
    {
        public OAuth1WebFacade(
            string consumerKey, 
            string consumerSecret, 
            string callbackUrl,
            string userId,
            IOAuthSecurityStrategy securityStrategy) : 
                base(
                    new TResourceProvider(), 
                    consumerKey, 
                    consumerSecret, 
                    callbackUrl, 
                    userId,
                    new OAuth1Authentication(),
                    securityStrategy)
        { }

        public OAuth1WebFacade(
            string consumerKey,
            string consumerSecret,
            string userId,
            string callbackUri) :
                base(
                    new TResourceProvider(),
                    consumerKey,
                    consumerSecret,
                    userId,
                    callbackUri,
                    new OAuth1Authentication(),
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(), 
                        TimeSpan.FromMinutes(2)))
        { }

        public OAuth1Credentials ParseAndValidateCallback(
            Uri responseUri)
        {
            var handler = new OAuth1CallbackHandler(
                _securityStrategy,
                OAuth1ParameterEnum.OAuthToken.EnumToString(),
                _userId);

            return handler.ParseAndValidateCallback<OAuth1Credentials>(
                responseUri);
        }
    }
}
