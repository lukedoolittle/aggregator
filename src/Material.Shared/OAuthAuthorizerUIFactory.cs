using System;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
#if !__WINDOWS__
using Material.View;
using Material.View.WebAuthorization;
#endif

namespace Material
{
    public class OAuthAuthorizerUIFactory : IOAuthAuthorizerUIFactory
    {
        private readonly HttpServer _server;
        private readonly IBrowser _browser;

        public OAuthAuthorizerUIFactory(
            HttpServer server,
            IBrowser browser)
        {
            _server = server;
            _browser = browser;
        }

        public IOAuthAuthorizerUI GetAuthorizer<TResourceProvider>(
            AuthenticationInterfaceEnum browserType,
            IOAuthCallbackHandler callbackHandler)
            where TResourceProvider : ResourceProvider
        {
#if __ANDROID__
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI(
                        _server,
                        callbackHandler,
                        _browser);
                case AuthenticationInterfaceEnum.Embedded:
                    return new WebViewAuthorizerUI(callbackHandler);
                default:
                    throw new NotSupportedException();
            }
#elif __IOS__
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI(
                        _server, 
                        callbackHandler,
                        _browser);
                    break;
                case AuthenticationInterfaceEnum.Embedded:
                    return new UIWebViewAuthorizerUI(callbackHandler);
                    break;
                default:
                    throw new NotSupportedException();
            }
#elif WINDOWS_UWP
            //TODO: reenable this when we have implemented the HTTP server
            //in Foundations.Http.UWP
            if (browserType == AuthenticationInterfaceEnum.Dedicated)
            {
                throw new NotSupportedException();
            }

            return new WebViewAuthorizerUI(callbackHandler);
#elif __WINDOWS__
            return new BrowserAuthorizerUI(
                _server,
                callbackHandler,
                _browser);
#else
            throw new NotSupportedException();
#endif
        }
    }
}
