using System;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
#if !__WINDOWS__
using Material.View;
using Material.View.WebAuthorization;
#endif
#if __WINDOWS__
using Foundations.Http;
#endif

namespace Material.Infrastructure.OAuth
{
    public class OAuthAuthorizerUIFactory : IOAuthAuthorizerUIFactory
    {
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
                        new ProtocolOAuthCallbackListener(
                            callbackHandler),
                        Platform.Current);
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
                        new ProtocolOAuthCallbackListener(
                            callbackHandler),
                        Platform.Current);
                case AuthenticationInterfaceEnum.Embedded:
                    return new UIWebViewAuthorizerUI(callbackHandler);
                default:
                    throw new NotSupportedException();
            }
#elif WINDOWS_UWP
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI(
                        new ProtocolOAuthCallbackListener(
                            callbackHandler),
                        Platform.Current);
                case AuthenticationInterfaceEnum.Embedded:
                    return new WebViewAuthorizerUI(callbackHandler);
                default:
                    throw new NotSupportedException();
            }
#elif __WINDOWS__
            return new BrowserAuthorizerUI(
                new HttpOAuthCallbackListener(
                    new HttpServer(), 
                    callbackHandler), 
                Platform.Current);
#else
            throw new NotSupportedException();
#endif
        }
    }
}
