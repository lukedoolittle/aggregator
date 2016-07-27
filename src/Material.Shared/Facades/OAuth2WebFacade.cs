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
    public class OAuth2WebFacade<TResourceProvider> : OAuth2AuthenticationFacade
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        public OAuth2WebFacade(
            string clientId, 
            string userId, 
            string callbackUri, 
            IOAuthSecurityStrategy strategy) :
                base(
                    new TResourceProvider(), 
                    clientId, 
                    userId, 
                    callbackUri, 
                    new OAuth2Authentication(), 
                    strategy)
        { }

        public OAuth2WebFacade(
            string clientId,
            string userId,
            string callbackUri) :
                base(
                    new TResourceProvider(),
                    clientId,
                    userId,
                    callbackUri,
                    new OAuth2Authentication(),
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(), 
                        TimeSpan.FromMinutes(2)))
        { }

        public OAuth2Credentials ParseAndValidateCallback(
            Uri responseUri)
        {
            var handler = new OAuth2CallbackHandler(
                _strategy,
                OAuth2ParameterEnum.State.EnumToString(),
                _userId);

            return handler.ParseAndValidateCallback<OAuth2Credentials>(
                responseUri);
        }

        public OAuth2Credentials ParseAndValidateTokenCallback(
            Uri responseUri)
        {
            var handler = new OAuth2TokenCallbackHandler(
                _strategy,
                OAuth2ParameterEnum.State.EnumToString(),
                _userId);

            return handler.ParseAndValidateCallback<OAuth2Credentials>(
                responseUri);
        }
    }
}
