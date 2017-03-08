using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Canonicalizers;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;
using Material.OAuth.Template;

namespace Material.OAuth.Workflow
{
    public class OAuth1AppBase<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly AuthorizationInterface _browserType;
        private readonly TResourceProvider _provider;

        public OAuth1AppBase(
            string consumerKey,
            string consumerSecret,
            Uri callbackUri,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthorizationInterface browserType)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUri = callbackUri;
            _uiFactory = uiFactory;
            _provider = provider;
            _browserType = browserType;
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public virtual async Task<OAuth1Credentials> GetCredentialsAsync(
            string requestId)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            var handler = new OAuth1CallbackHandler(
                securityStrategy,
                OAuth1Parameter.OAuthToken.EnumToString());

            var facade = new OAuth1AuthorizationFacade(
                _provider,
                _consumerKey,
                _consumerSecret,
                _callbackUri,
                new OAuthAuthorizationAdapter(),
                securityStrategy,
                HmacDigestSigningAlgorithm.Sha1Algorithm(),
                new CryptoStringGenerator(),
                new OAuth1Canonicalizer());

            var authorizationUi = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth1Credentials>(
                    _browserType,
                    handler,
                    _callbackUri);

            var template = new OAuthAuthorizationTemplate<OAuth1Credentials>(
                authorizationUi,
                facade,
                facade,
                securityStrategy);

            var result = await template
                .GetAccessTokenCredentials(requestId)
                .ConfigureAwait(false);

            securityStrategy.ClearSecureParameters(requestId);

            return result;
        }
    }
}
