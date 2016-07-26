using System;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.OAuth;
#if __MOBILE__
#endif
#if __ANDROID__
using Material.View.WebAuthorization;
#endif

namespace Material
{
    public class OAuthAuthorizerUIFactory : IOAuthAuthorizerUIFactory
    {
        private readonly HttpServer _server;

        public OAuthAuthorizerUIFactory(
            HttpServer server)
        {
            _server = server;
        }

        public IOAuthAuthorizerUI GetAuthorizer<TResourceProvider>(
            AuthenticationInterfaceEnum browserType,
            OAuthCallbackHandler callbackHandler)
            where TResourceProvider : ResourceProvider
        {
#if __ANDROID__
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI(
                        _server,
                        callbackHandler);
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
                        callbackHandler);
                    break;
                case AuthenticationInterfaceEnum.Embedded:
                    return new UIWebViewAuthorizerUI(callbackHandler);
                    break;
                default:
                    throw new NotSupportedException();
            }
#else
            if (browserType == AuthenticationInterfaceEnum.Embedded)
            {
                throw new NotSupportedException();
            }

            return new BrowserAuthorizerUI(
                _server,
                callbackHandler);
#endif
        }
    }
}
