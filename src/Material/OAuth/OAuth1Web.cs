using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth;

namespace Material.Facades
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth1a
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth1Web<TResourceProvider> : OAuth1AuthenticationFacade
        where TResourceProvider: OAuth1ResourceProvider, new()
    {
        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="securityStrategy">The security strategy to use for token and secret handling</param>
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

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow with default security strategy
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
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

        /// <summary>
        /// Convert a callback uri into OAuth1Credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Intermediate OAuth1 credentials</returns>
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
