using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.OAuth;
using Xamarin.Forms;

namespace Material
{
    //In theory then have to register the IOAuthFactory


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
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
            base(
                consumerKey,
                consumerSecret,
                callbackUrl,
                DependencyService.Get<IOAuthAuthorizerUIFactory>(),
                new TResourceProvider(),
                browserType)
        { }
    }
}
