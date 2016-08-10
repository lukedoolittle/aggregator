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
            TResourceProvider provider = null,
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
            this(
                clientId, 
                null, 
                callbackUrl,
                uiFactory,
                provider, 
                browserType)
        { }

        public OAuth2AppBase(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider = null,
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _callbackUrl = callbackUrl;
            _browserType = browserType;
            _uiFactory = uiFactory;
            _provider = provider ?? new TResourceProvider();

            _provider.SetClientProperties(clientId, clientSecret);
        }

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

            IOAuthAuthenticationTemplate<OAuth2Credentials> template = null;
            switch (_provider.Flow)
            {
                case ResponseTypeEnum.Code:
                    template = builder.BuildOAuth2CodeTemplate<TResourceProvider>(
                        facade,
                        _browserType,
                        userId,
                        _clientSecret);
                    break;
                case ResponseTypeEnum.Token:
                    template = builder.BuildOAuth2TokenTemplate<TResourceProvider>(
                        facade,
                        _browserType,
                        userId);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return template.GetAccessTokenCredentials(userId);
        }

        public OAuth2AppBase<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _provider.AddRequestScope<TRequest>();
            return this;
        }
    }
}
