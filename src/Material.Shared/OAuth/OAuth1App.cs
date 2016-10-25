using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.OAuth;
#if __FORMS__
using Xamarin.Forms;
#endif

namespace Material.Infrastructure.OAuth
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth1a
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth1App<TResourceProvider> : OAuth1AppBase<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterface browserType = AuthenticationInterface.Dedicated
#else
            AuthenticationInterface browserType = AuthenticationInterface.Embedded
#endif
            ) :
                base(
                    consumerKey,
                    consumerSecret,
                    new Uri(callbackUrl),
#if __FORMS__
                    DependencyService.Get<IOAuthAuthorizerUIFactory>(),
#else
                    new OAuthAuthorizerUIFactory(),
#endif
                    new TResourceProvider(),
                    browserType)
        { }
    }
}
