using Material.Infrastructure;
using Material.Infrastructure.OAuth;
using Material.OAuth;

namespace Material.Facades
{
    public class OAuth1WebFacade<TResourceProvider> : OAuth1AuthenticationFacade
        where TResourceProvider: OAuth1ResourceProvider, new()
    {
        public OAuth1WebFacade(
            string consumerKey, 
            string consumerSecret, 
            string callbackUrl) : 
                base(
                    new TResourceProvider(), 
                    consumerKey, 
                    consumerSecret, 
                    callbackUrl, 
                    new OAuth1Authentication())
        { }
    }
}
