using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth;

namespace Material.Facades
{
    public class OAuth1Web<TResourceProvider> : OAuth1AuthenticationFacade
        where TResourceProvider: OAuth1ResourceProvider, new()
    {
        public OAuth1Web(
            string consumerKey, 
            string consumerSecret, 
            string callbackUrl,
            IOAuthSecurityStrategy securityStrategy) : 
                base(
                    new TResourceProvider(), 
                    consumerKey, 
                    consumerSecret, 
                    callbackUrl,
                    new OAuth1Authentication(), 
                    securityStrategy)
        { }

        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            string callbackUri) :
                base(
                    new TResourceProvider(),
                    consumerKey,
                    consumerSecret,
                    callbackUri,
                    new OAuth1Authentication(),
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(), 
                        TimeSpan.FromMinutes(2)))
        { }

        public OAuth1Credentials ParseAndValidateCallback(
            Uri responseUri,
            string userId)
        {
            var handler = new OAuth1CallbackHandler(
                _securityStrategy,
                OAuth1ParameterEnum.OAuthToken.EnumToString(),
                userId);

            return handler.ParseAndValidateCallback<OAuth1Credentials>(
                responseUri);
        }
    }
}
