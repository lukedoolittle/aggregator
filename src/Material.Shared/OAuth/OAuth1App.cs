using System.Threading.Tasks;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;

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
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) :
                base(
                    consumerKey,
                    consumerSecret,
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(),
                    new TResourceProvider(),
                    browserType)
        { }
    }
}
