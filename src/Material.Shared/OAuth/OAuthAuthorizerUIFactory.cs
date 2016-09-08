using System;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
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
        public IOAuthAuthorizerUI<TCredentials> GetAuthorizer<TResourceProvider, TCredentials>(
            AuthenticationInterfaceEnum browserType,
            IOAuthCallbackHandler<TCredentials> callbackHandler,
            Uri callbackUri)
            where TResourceProvider : ResourceProvider
            where TCredentials : TokenCredentials
        {
#if __ANDROID__
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI<TCredentials>(
                        new ProtocolOAuthCallbackListener<TCredentials>(
                            callbackHandler),
                        Platform.Current,
                        callbackUri);
                case AuthenticationInterfaceEnum.Embedded:
                    return new WebViewAuthorizerUI<TCredentials>(
                        callbackHandler,
                        callbackUri);
                default:
                    throw new NotSupportedException();
            }
#elif __IOS__
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI<TCredentials>(
                        new ProtocolOAuthCallbackListener<TCredentials>(
                            callbackHandler),
                        Platform.Current,
                        callbackUri);
                case AuthenticationInterfaceEnum.Embedded:
                    return new UIWebViewAuthorizerUI<TCredentials>(
                        callbackHandler,
                        callbackUri);
                default:
                    throw new NotSupportedException();
            }
#elif WINDOWS_UWP
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserAuthorizerUI<TCredentials>(
                        new ProtocolOAuthCallbackListener<TCredentials>(
                            callbackHandler),
                        Platform.Current);
                case AuthenticationInterfaceEnum.Embedded:
                    return new WebViewAuthorizerUI<TCredentials>(callbackHandler);
                default:
                    throw new NotSupportedException();
            }
#elif __WINDOWS__
            return new BrowserAuthorizerUI<TCredentials>(
                new HttpOAuthCallbackListener<TCredentials>(
                    new HttpServer(), 
                    callbackHandler), 
                Platform.Current,
                callbackUri);
#else
            throw new NotSupportedException();
#endif
        }
    }
}
