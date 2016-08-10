using Foundations.Http;
using Material.Enums;

namespace Material.Infrastructure.OAuth
{
    public class OAuth1App<TResourceProvider> : OAuth1AppBase<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        public OAuth1App(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) : 
                base(
                    consumerKey, 
                    consumerSecret, 
                    callbackUrl, 
                    new OAuthAuthorizerUIFactory(
                        new HttpServer()), 
                    browserType)
        { }
    }

    public class OAuth2App<TResourceProvider> : OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {

        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider = null,
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
            this(
                clientId,
                null,
                callbackUrl,
                provider,
                browserType)
        { }

        public OAuth2App(
            string clientId,
            string clientSecret,
            string callbackUrl,
            TResourceProvider provider = null,
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) : 
                base(
                    clientId, 
                    clientSecret, 
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(
                        new HttpServer()), 
                    provider,
                    browserType)
        { }
    }
}
