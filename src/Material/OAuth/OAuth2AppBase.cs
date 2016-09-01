using System;
using System.Threading.Tasks;
using Material.Infrastructure.OAuth;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Builder;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _callbackUrl;
        private readonly AuthenticationInterfaceEnum _browserType;
        private readonly IOAuth2TemplateBuilder _builder;
        protected readonly TResourceProvider _provider;

        public OAuth2AppBase(
            string clientId,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthenticationInterfaceEnum browserType)
        {
            _clientId = clientId;
            _callbackUrl = callbackUrl;
            _browserType = browserType;
            _provider = provider;

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            _builder =
                new OAuth2TokenTemplateBuilder(
                    uiFactory,
                    null,
                    securityStrategy);
        }

        public OAuth2AppBase(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthenticationInterfaceEnum browserType)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _callbackUrl = callbackUrl;
            _browserType = browserType;
            _provider = provider;

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            _builder =
                new OAuth2CodeTemplateBuilder(
                    uiFactory,
                    null,
                    securityStrategy);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow
        /// </summary>
        /// <returns></returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync()
        {
            var userId = Guid.NewGuid().ToString();

            var facade = _builder.BuildFacade(
                _provider,
                new OAuth2Authentication(), 
                _clientId,
                _callbackUrl,
                _browserType);

            var template = _builder.BuildTemplate<TResourceProvider>(
                    facade,
                    _browserType,
                    userId,
                    _clientSecret);

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
