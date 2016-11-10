using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth.Callback;
using Material.OAuth.Security;

#if __FORMS__
using Xamarin.Forms;
#endif

namespace Material.OAuth
{
    /// <summary>
    /// Authorizes a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authorize with</typeparam>
    public class OAuth2App<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly OAuth2AppBase<TResourceProvider> _app;
        private readonly IOAuthSecurityStrategy _securityStrategy;
#if !__WINDOWS__
        private readonly AuthorizationInterface _browserType;
#endif

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
            AuthorizationInterface browserType = AuthorizationInterface.Dedicated
#else
            AuthorizationInterface browserType = AuthorizationInterface.Embedded
#endif
            )
        {
#if !__WINDOWS__
            _browserType = browserType;
#endif

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            _app = new OAuth2AppBase<TResourceProvider>(
                clientId, 
                new Uri(callbackUrl),
#if __FORMS__
                    DependencyService.Get<IOAuthAuthorizerUIFactory>(),
#else
                    new OAuthAuthorizerUIFactory(),
#endif
                _securityStrategy, 
                provider, 
                browserType);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OAuth2App(
            string clientId,
            string callbackUrl,
#if __WINDOWS__
            AuthorizationInterface browserType = AuthorizationInterface.Dedicated
#else
            AuthorizationInterface browserType = AuthorizationInterface.Embedded
#endif
            ) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret)
        {
            var handler = new OAuth2CallbackHandler(
                _securityStrategy,
                OAuth2Parameter.State.EnumToString());

            return _app.GetCredentialsAsync(
                clientSecret, 
                OAuth2ResponseType.Code, 
                handler);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            var handler = new OAuth2CallbackHandler(
                _securityStrategy,
                OAuth2Parameter.State.EnumToString());

#if !__WINDOWS__
            //This is sort of a bizarre hack: Google requires that you go through the
            //code workflow with a mobile device even if you don't have a client secret
            if (_browserType == AuthorizationInterface.Dedicated &&
                typeof(TResourceProvider) == typeof(Google))
            {
                return _app.GetCredentialsAsync(
                        null,
                        OAuth2ResponseType.Code,
                        handler);
            }
#endif

            return _app.GetCredentialsAsync(
                OAuth2ResponseType.Token,
                handler);
        }


        /// <summary>
        /// Adds scope to be requested with OAuth2 authorization
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2App<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _app.AddScope<TRequest>();

            return this;
        }
    }
}
