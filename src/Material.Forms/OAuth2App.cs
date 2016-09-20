using System;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.OAuth;
using Xamarin.Forms;

namespace Material
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
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
                base(
                    clientId,
                    callbackUrl,
                    DependencyService.Get<IOAuthAuthorizerUIFactory>(),
                    provider,
                    browserType)
        {
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
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
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
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
                base(
                    clientId,
                    clientSecret,
                    callbackUrl,
                    DependencyService.Get<IOAuthAuthorizerUIFactory>(),
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
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded) :
                this(
                    clientId,
                    clientSecret,
                    callbackUrl,
                    new TResourceProvider(),
                    browserType)
        { }
    }
}
