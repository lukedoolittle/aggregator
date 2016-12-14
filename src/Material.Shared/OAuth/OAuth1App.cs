using System;
using System.Threading.Tasks;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Workflow;
#if __FORMS__
using Xamarin.Forms;
using Material.Contracts;
#endif

namespace Material.OAuth
{
    /// <summary>
    /// Authorizes a resource owner with the given resource provider using OAuth1a
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authorize with</typeparam>
    public class OAuth1App<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        private readonly OAuth1AppBase<TResourceProvider> _app;

        /// <summary>
        /// Authorizes a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            AuthorizationInterface browserType)
        {
            var provider = new TResourceProvider();

#if __WINDOWS__
            var @interface = AuthorizationInterface.NotSpecified;
#else
            var @interface = new AuthenticationUISelector(
                    Framework.Platform.Current.CanProvideSecureBrowsing)
                .GetOptimalOAuth1Interface(
                    provider,
                    browserType);
#endif

            _app = new OAuth1AppBase<TResourceProvider>(
                consumerKey,
                consumerSecret,
                new Uri(callbackUrl),
#if __FORMS__
                DependencyService.Get<IOAuthAuthorizerUIFactory>(),
#else
                new OAuthAuthorizerUIFactory(),
#endif
                provider,
                @interface);
        }

        /// <summary>
        /// Authorizes a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUrl) :
                this(
                    consumerKey,
                    consumerSecret,
                    callbackUrl,
                    AuthorizationInterface.NotSpecified)
        { }

        /// <summary>
        /// Authorizes a resource owner using the OAuth1a workflow
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public virtual Task<OAuth1Credentials> GetCredentialsAsync()
        {
            return _app.GetCredentialsAsync();
        }
    }
}
