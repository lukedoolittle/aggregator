using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
using Material.OAuth.Callback;
using Material.OAuth.Security;
using Xamarin.Forms;

namespace Material
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
            AuthenticationInterfaceEnum browserType)
        {
            _browserType = browserType;

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            _app = new OAuth2AppBase<TResourceProvider>(
                clientId,
                callbackUrl,
                DependencyService.Get<IOAuthAuthorizerUIFactory>(),
                _securityStrategy,
                provider,
                browserType);
        }

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider) : 
                this(
                    clientId, 
                    callbackUrl, 
                    provider, 
                    AuthenticationInterfaceEnum.Embedded)
        { }

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth2App(
            string clientId,
            string callbackUrl,
            AuthenticationInterfaceEnum browserType) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth2App(
            string clientId,
            string callbackUrl) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                AuthenticationInterfaceEnum.Embedded)
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
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
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
