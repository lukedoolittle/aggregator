using System;
using System.Threading.Tasks;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;

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
                    browserType,
#if __WINDOWS__
                    OAuthAppTypeEnum.Desktop
#else
                    OAuthAppTypeEnum.Mobile
#endif
            )
        { }

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow. 
        /// Throws a NoConnectivityException if the platform does not have
        /// internet access.
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2App<TResourceProvider> : OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        public OAuth2App(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
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

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        public OAuth2App(
            string clientId,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) :
            this(
                clientId,
                null,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        public OAuth2App(
            string clientId,
            string clientSecret,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) : 
                base(
                    clientId, 
                    clientSecret, 
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(), 
                    provider,
                    browserType,
#if __WINDOWS__
                    OAuthAppTypeEnum.Desktop
#else
                    OAuthAppTypeEnum.Mobile
#endif
            )
        { }

        /// <summary>
        /// Authenticate a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        public OAuth2App(
            string clientId,
            string clientSecret,
            string callbackUrl,
#if __WINDOWS__
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Dedicated
#else
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded
#endif
            ) :
                base(
                    clientId,
                    clientSecret,
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(),
                    new TResourceProvider(), 
                    browserType,
#if __WINDOWS__
                    OAuthAppTypeEnum.Desktop
#else
                    OAuthAppTypeEnum.Mobile
#endif
            )
        { }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow
        /// Throws a NoConnectivityException if the platform does not have
        /// internet access.
        /// </summary>
        /// <returns></returns>
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
