using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;
using Material.OAuth.Template;

namespace Material.OAuth
{
    public class OAuth1AppBase<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly AuthenticationInterface _browserType;
        private readonly TResourceProvider _provider;

        public OAuth1AppBase(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthenticationInterface browserType)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUri = new Uri(callbackUrl);
            _uiFactory = uiFactory;
            _provider = provider;
            _browserType = browserType;
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <returns></returns>
        public virtual Task<OAuth1Credentials> GetCredentialsAsync()
        {
            var userId = Guid.NewGuid().ToString();

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var handler = new OAuth1CallbackHandler(
                securityStrategy,
                OAuth1Parameter.OAuthToken.EnumToString());

            var facade = new OAuth1AuthenticationFacade(
                _provider,
                _consumerKey,
                _consumerSecret,
                _callbackUri,
                new OAuth1AuthenticationAdapter(),
                securityStrategy);

            var authenticationUI = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth1Credentials>(
                    _browserType,
                    handler,
                    _callbackUri);

            var template = new OAuth1AuthenticationTemplate(
                authenticationUI,
                facade,
                securityStrategy,
                userId);

            return template.GetAccessTokenCredentials(userId);
        }
    }
}
