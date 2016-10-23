using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Callback;
using Material.Infrastructure.ProtectedResources;

namespace Material.Infrastructure.OAuth
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2App<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly OAuth2AppBase<TResourceProvider> _app;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly AuthenticationInterfaceEnum _browserType;

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            )
        {
            _browserType = browserType;

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            _app = new OAuth2AppBase<TResourceProvider>(
                clientId, 
                callbackUrl, 
                new OAuthAuthorizerUIFactory(), 
                _securityStrategy, 
                provider, 
                browserType);
        }

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        public OAuth2App(
            string clientId,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret)
        {
            var handler = new OAuth2QueryCallbackHandler(
                _securityStrategy,
                OAuth2Parameter.State.EnumToString());

            return _app.GetCredentialsAsync(
                clientSecret, 
                OAuth2ResponseType.Code, 
                handler);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
#if __WINDOWS__
            return _app.GetCredentialsAsync(
                    OAuth2ResponseType.Token, 
                    new OAuth2QueryCallbackHandler(
                        _securityStrategy,
                        OAuth2Parameter.State.EnumToString()));

#else
            //This is sort of a bizarre hack because Google requires that you go through the
            //code workflow with a mobile device even if you don't have a client secret
            if (_browserType == AuthenticationInterfaceEnum.Dedicated && 
                typeof(TResourceProvider) == typeof(Google))
            {
                return _app.GetCredentialsAsync(
                        null, 
                        OAuth2ResponseType.Code, 
                        new OAuth2QueryCallbackHandler(
                            _securityStrategy,
                            OAuth2Parameter.State.EnumToString()));
            }
            else
            {
                return _app.GetCredentialsAsync(
                        OAuth2ResponseType.Token,
                        new OAuth2FragmentCallbackHandler(
                            _securityStrategy,
                            OAuth2Parameter.State.EnumToString()));
            }
#endif
        }


        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        public OAuth2App<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _app.AddScope<TRequest>();

            return this;
        }
    }
}
