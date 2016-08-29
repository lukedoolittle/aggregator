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
        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="strategy">The security strategy to use for "state" handling</param>
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

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUri">The application's registered callback url</param>
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

        /// <summary>
        /// Convert a callback uri into OAuth1Credentials using "Code" workflow
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Intermediate OAuth2 credentials</returns>
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

        /// <summary>
        /// Convert a callback uri into OAuth1Credentials using "Token" workflow
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>OAuth2 credentials</returns>
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
