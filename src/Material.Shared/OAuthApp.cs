using System.Threading.Tasks;
using Foundations.Http;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;

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

        public override Task<OAuth1Credentials> GetCredentialsAsync()
        {
            if (!Platform.IsOnline)
            {
                throw new NoConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            return base.GetCredentialsAsync();
        }
    }

    public class OAuth2App<TResourceProvider> : OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {

        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider = null,
#if !__MOBILE__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) :
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
#if !__MOBILE__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) : 
                base(
                    clientId, 
                    clientSecret, 
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(
                        new HttpServer()), 
                    provider,
                    browserType)
        { }

        public override Task<OAuth2Credentials> GetCredentialsAsync()
        {
            if (!Platform.IsOnline)
            {
                throw new NoConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            return base.GetCredentialsAsync();
        }
    }
}
