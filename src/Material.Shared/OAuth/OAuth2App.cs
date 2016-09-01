using System;
using System.Threading.Tasks;
using Foundations.HttpClient.Enums;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
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
                base(
                    clientId,
                    callbackUrl,
                    new OAuthAuthorizerUIFactory(),
                    provider, 
                    browserType)
        {
#if __WINDOWS__
            _provider.SetFlow(ResponseTypeEnum.Token);
#else
            if (browserType == AuthenticationInterfaceEnum.Dedicated)
            {
                _provider.SetFlow(ResponseTypeEnum.Code);
            }
            else if (browserType == AuthenticationInterfaceEnum.Embedded)
            {
                _provider.SetFlow(ResponseTypeEnum.Token);
            }
            else
            {
                throw new NotSupportedException();
            }
#endif
        }

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
                    browserType)
        {
            _provider.SetFlow(ResponseTypeEnum.Code);
        }

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
                this(
                    clientId,
                    clientSecret,
                    callbackUrl,
                    new TResourceProvider(), 
                    browserType)
        { }
    }
}
