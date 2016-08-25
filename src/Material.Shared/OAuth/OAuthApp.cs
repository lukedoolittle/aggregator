using System;
using System.Threading.Tasks;
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
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Desktop
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Mobile
#endif
            ) : 
                base(
                    consumerKey, 
                    consumerSecret, 
                    callbackUrl, 
                    new OAuthAuthorizerUIFactory(), 
                    new TResourceProvider(), 
                    browserType,
                    appType)
        { }

        public override Task<OAuth1Credentials> GetCredentialsAsync()
        {
            if (!Platform.Current.IsOnline)
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
            TResourceProvider provider,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Desktop
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Mobile
#endif
            ) :
            this(
                clientId,
                null,
                callbackUrl,
                provider,
                browserType,
                appType)
        { }

        public OAuth2App(
            string clientId,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Desktop
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Mobile
#endif
            ) :
            this(
                clientId,
                null,
                callbackUrl,
                new TResourceProvider(),
                browserType,
                appType)
        { }

        public OAuth2App(
            string clientId,
            string clientSecret,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Desktop
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Mobile
#endif
            ) : 
                base(
                    clientId, 
                    clientSecret, 
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(), 
                    provider,
                    browserType,
                    appType)
        { }

        public OAuth2App(
            string clientId,
            string clientSecret,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Desktop
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded,
            OAuthAppTypeEnum appType = OAuthAppTypeEnum.Mobile
#endif
            ) :
                base(
                    clientId,
                    clientSecret,
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(),
                    new TResourceProvider(), 
                    browserType,
                    appType)
        { }

        public override Task<OAuth2Credentials> GetCredentialsAsync()
        {
            if (!Platform.Current.IsOnline)
            {
                throw new NoConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            return base.GetCredentialsAsync();
        }
    }
}
