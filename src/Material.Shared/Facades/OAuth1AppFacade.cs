using System;
using System.Threading.Tasks;
using Material.OAuth;
using Foundations.Http;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Task;

namespace Material.Infrastructure.OAuth
{
    public class OAuth1AppFacade<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _callbackUrl;
        private readonly AuthenticationInterfaceEnum _browserType;

        public OAuth1AppFacade(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
#if !__MOBILE__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            )
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUrl = callbackUrl;
            _browserType = browserType;
        }

        public Task<OAuth1Credentials> GetOAuth1Credentials()
        {
            var userId = Guid.NewGuid().ToString();

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var builder =
                new OAuthBuilder(
                    new OAuthAuthorizerUIFactory(
                        new HttpServer()),
                    null,
                    new OAuthFactory(),
                    securityStrategy);
            var facade = builder.BuildOAuth1Facade(
                new TResourceProvider(),
                _consumerKey,
                _consumerSecret, 
                userId,
                _callbackUrl);
            var template = builder.BuildOAuth1Template<TResourceProvider>(
                facade,
                _browserType,
                userId);

            return template.GetAccessTokenCredentials();
        }
    }
}
