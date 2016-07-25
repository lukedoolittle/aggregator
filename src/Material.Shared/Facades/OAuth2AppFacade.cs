using System;
using System.Threading.Tasks;
using Material.OAuth;
using Foundations.Http;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Task;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AppFacade<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _callbackUrl;
        private readonly AuthenticationInterfaceEnum _browserType;
        private readonly TResourceProvider _provider;

        public OAuth2AppFacade(
            string clientId,
            string callbackUrl,
            TResourceProvider provider = null,
#if !__MOBILE__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) : this(
                clientId, 
                null, 
                callbackUrl, 
                provider, 
                browserType)
        { }

        public OAuth2AppFacade(
            string clientId,
            string clientSecret,
            string callbackUrl,
            TResourceProvider provider = null,
#if !__MOBILE__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            )
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _callbackUrl = callbackUrl;
            _browserType = browserType;
            _provider = provider ?? new TResourceProvider();
        }

        public Task<OAuth2Credentials> GetOAuth2Credentials()
        {
            var userId = Guid.NewGuid().ToString();

            var securityAlgorithm = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var builder =
                new OAuthBuilder(
                    new OAuthAuthorizerUIFactory(
                        new HttpServer()),
                    null,
                    new OAuthFactory(),
                    securityAlgorithm);

            var facade = builder.BuildOAuth2Facade(
                _provider,
                _clientId,
                _clientSecret,
                userId,
                _callbackUrl);

            OAuthAuthenticationTemplate<OAuth2Credentials> template = null;
            switch (_provider.Flow)
            {
                case ResponseTypeEnum.Token:
                    template = builder.BuildOAuth2TokenTemplate<TResourceProvider>(
                        facade,
                        _browserType,
                        userId);
                    break;
                case ResponseTypeEnum.Code:
                    template = builder.BuildOAuth2CodeTemplate<TResourceProvider>(
                        facade,
                        _browserType,
                        userId);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return template.GetAccessTokenCredentials();
        }

        public OAuth2AppFacade<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _provider.AddRequestScope<TRequest>();
            return this;
        }
    }
}
