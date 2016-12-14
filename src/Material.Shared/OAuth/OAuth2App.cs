using System;
using System.Threading.Tasks;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Workflow;

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

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
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
            AuthorizationInterface browserType
            )
        {
#if __WINDOWS__
            var @interface = AuthorizationInterface.NotSpecified;
#else
            var @interface = new AuthenticationUISelector(
                Framework.Platform.Current.CanProvideSecureBrowsing)
                .GetOptimalOAuth2Interface(
                    provider, 
                    browserType);
#endif

            _app = new OAuth2AppBase<TResourceProvider>(
                clientId,
                new Uri(callbackUrl),
#if __FORMS__
                    Xamarin.Forms.DependencyService.Get<Contracts.IOAuthAuthorizerUIFactory>(),
#else
                    new OAuthAuthorizerUIFactory(),
#endif
                provider,
                @interface,
                Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
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
                AuthorizationInterface.NotSpecified)
        {}

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth2App(
            string clientId,
            string callbackUrl,
            AuthorizationInterface browserType) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth2App(
            string clientId,
            string callbackUrl) :
            this(
                clientId,
                callbackUrl,
                AuthorizationInterface.NotSpecified)
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret)
        {
            return _app.GetCredentialsAsync(clientSecret);
        }

        /// <summary>
        /// Authorize a resource owner using a mobile workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            return _app.GetCredentialsAsync();
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

        /// <summary>
        /// Adds scope to be requested with OAuth2 authorization
        /// </summary>
        /// <param name="scope">The scope to request</param>
        /// <returns>The current instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2App<TResourceProvider> AddScope(string scope)
        {
            _app.AddScope(scope);

            return this;
        }
    }
}
