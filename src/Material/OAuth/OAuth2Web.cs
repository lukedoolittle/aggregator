using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth;

namespace Material.Facades
{
    public class OAuth2Web<TResourceProvider> : OAuth2AuthenticationFacade
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        public OAuth2Web(
            string clientId, 
            string callbackUri, 
            IOAuthSecurityStrategy strategy) :
                base(
                    new TResourceProvider(), 
                    clientId, 
                    callbackUri, 
                    new OAuth2Authentication(), 
                    strategy)
        { }

        public OAuth2Web(
            string clientId,
            string callbackUri) :
                base(
                    new TResourceProvider(),
                    clientId,
                    callbackUri,
                    new OAuth2Authentication(),
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(), 
                        TimeSpan.FromMinutes(2)))
        { }

        public OAuth2Credentials ParseAndValidateCallback(
            Uri responseUri,
            string userId)
        {
            var handler = new OAuth2QueryCallbackHandler(
                _strategy,
                OAuth2ParameterEnum.State.EnumToString(),
                userId);

            return handler.ParseAndValidateCallback<OAuth2Credentials>(
                responseUri);
        }

        public OAuth2Credentials ParseAndValidateTokenCallback(
            Uri responseUri,
            string userId)
        {
            var handler = new OAuth2FragmentCallbackHandler(
                _strategy,
                OAuth2ParameterEnum.State.EnumToString(),
                userId);

            return handler.ParseAndValidateCallback<OAuth2Credentials>(
                responseUri);
        }
    }
}
