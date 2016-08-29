using System;
using System.Threading.Tasks;
using Material.OAuth;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _callbackUrl;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly AuthenticationInterfaceEnum _browserType;
        private readonly TResourceProvider _provider;

        public OAuth2AppBase(
            string clientId,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthenticationInterfaceEnum browserType,
            OAuthAppTypeEnum appType) :
            this(
                clientId, 
                null, 
                callbackUrl,
                uiFactory,
                provider, 
                browserType,
                appType)
        { }

        public OAuth2AppBase(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthenticationInterfaceEnum browserType,
            OAuthAppTypeEnum appType)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _callbackUrl = callbackUrl;
            _browserType = browserType;
            _uiFactory = uiFactory;
            _provider = provider;

            provider.SetFlow(
                clientId, 
                clientSecret, 
                browserType, 
                appType);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow
        /// </summary>
        /// <returns></returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync()
        {
            var userId = Guid.NewGuid().ToString();

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var builder =
                new OAuthBuilder(
                    _uiFactory,
                    null,
                    securityStrategy);

            var facade = builder.BuildOAuth2Facade(
                _provider,
                new OAuth2Authentication(), 
                _clientId,
                _clientSecret,
                _callbackUrl);

            var template = builder.BuildOAuth2Template<TResourceProvider>(
                    facade,
                    _browserType,
                    userId,
                    _clientSecret,
                    _provider.Flow);

            return template.GetAccessTokenCredentials(userId);
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        public OAuth2AppBase<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _provider.AddRequestScope<TRequest>();

            return this;
        }
    }
}
