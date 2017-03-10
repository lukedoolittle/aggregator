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

namespace Material.Application
{
    /// <summary>
    /// Authorizes a resource owner with the given resource provider using OAuth1a
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authorize with</typeparam>
    public class OAuth1App<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly IOAuthCallbackHandler<OAuth1Credentials> _callbackHandler;
        private readonly IOAuthAuthorizationAdapter _authAdapter;
        private readonly ICryptoStringGenerator _cryptoStringGenerator;
        private readonly IHttpRequestCanonicalizer _canonicalizer;
        private readonly ISigningAlgorithm _signingAlgorithm;
        private readonly AuthorizationInterface _browserType;
        private readonly TResourceProvider _provider;

        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            Uri callbackUri,
            IOAuthAuthorizerUIFactory uiFactory,
            IOAuthSecurityStrategy securityStrategy,
            IOAuthCallbackHandler<OAuth1Credentials> callbackHandler,
            IOAuthAuthorizationAdapter authAdapter,
            ICryptoStringGenerator cryptoStringGenerator,
            IHttpRequestCanonicalizer canonicalizer,
            ISigningAlgorithm signingAlgorithm,
            IAuthorizationUISelector authorizerSelector,
            TResourceProvider provider,
            AuthorizationInterface browserType)
        {
            if (uiFactory == null) throw new ArgumentNullException(nameof(uiFactory), StringResources.InitializationError);
            if (authorizerSelector == null) throw new ArgumentNullException(nameof(authorizerSelector), StringResources.InitializationError);

            if (consumerKey == null) throw new ArgumentNullException(nameof(consumerKey));
            if (consumerSecret == null) throw new ArgumentNullException(nameof(consumerSecret));
            if (callbackUri == null) throw new ArgumentNullException(nameof(callbackUri));
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (callbackHandler == null) throw new ArgumentNullException(nameof(callbackHandler));
            if (cryptoStringGenerator == null) throw new ArgumentNullException(nameof(cryptoStringGenerator));
            if (canonicalizer == null) throw new ArgumentNullException(nameof(canonicalizer));
            if (signingAlgorithm == null) throw new ArgumentNullException(nameof(signingAlgorithm));
            if (authAdapter == null) throw new ArgumentNullException(nameof(authAdapter));
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUri = callbackUri;
            _uiFactory = uiFactory;
            _provider = provider;
            _securityStrategy = securityStrategy;
            _callbackHandler = callbackHandler;
            _authAdapter = authAdapter;
            _cryptoStringGenerator = cryptoStringGenerator;
            _canonicalizer = canonicalizer;
            _signingAlgorithm = signingAlgorithm;

            _browserType = authorizerSelector.GetOptimalOAuth1Interface(
                provider,
                browserType);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUri,
            IOAuthSecurityStrategy securityStrategy,
            AuthorizationInterface browserType) : 
                this(
                    consumerKey, 
                    consumerSecret, 
                    new Uri(callbackUri), 
                    QuantfabricConfiguration.WebAuthorizationUIFactory,
                    securityStrategy,
                    new OAuth1CallbackHandler(
                        securityStrategy,
                        OAuth1Parameter.OAuthToken.EnumToString()), 
                    new OAuthAuthorizationAdapter(), 
                    new CryptoStringGenerator(),
                    new OAuth1Canonicalizer(),
                    HmacDigestSigningAlgorithm.Sha1Algorithm(),
                    QuantfabricConfiguration.WebAuthenticationUISelector,
                    new TResourceProvider(), 
                    browserType)
        { }

        /// <summary>
        /// Authorizes a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUri) : 
                this(
                    consumerKey, 
                    consumerSecret, 
                    callbackUri, 
                    AuthorizationInterface.NotSpecified)
        { }

        /// <summary>
        /// Authorizes a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUri,
            AuthorizationInterface browserType) :
                this(
                    consumerKey,
                    consumerSecret,
                    callbackUri,
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        QuantfabricConfiguration.SecurityParameterTimeout),
                    browserType)
        { }

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <returns>Valid OAuth1 credentials</returns>
        public Task<OAuth1Credentials> GetCredentialsAsync()
        {
            return GetCredentialsAsync(
                _cryptoStringGenerator.CreateRandomString());
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="requestId">A unique requestId NOTE THIS MUST AT LEAST UNIQUE PER USER</param>
        /// <returns>Valid OAuth1 credentials</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public async Task<OAuth1Credentials> GetCredentialsAsync(
            string requestId)
        {
            var facade = new OAuth1AuthorizationFacade(
                _provider,
                _consumerKey,
                _consumerSecret,
                _callbackUri,
                _authAdapter,
                _securityStrategy,
                _signingAlgorithm,
                _cryptoStringGenerator,
                _canonicalizer);

            var authorizationUi = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth1Credentials>(
                    _browserType,
                    _callbackHandler,
                    _callbackUri);

            var template = new OAuthAuthorizationTemplate<OAuth1Credentials>(
                authorizationUi,
                facade,
                facade,
                _securityStrategy);

            var result = await template
                .GetAccessTokenCredentials(requestId)
                .ConfigureAwait(false);

            _securityStrategy.ClearSecureParameters(requestId);

            return result;
        }
    }
}
