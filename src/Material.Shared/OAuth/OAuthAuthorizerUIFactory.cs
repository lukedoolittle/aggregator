using System;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.OAuth;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public IOAuthAuthorizerUI<TCredentials> GetAuthorizer<TResourceProvider, TCredentials>(
            AuthorizationInterface browserType,
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri)
            where TResourceProvider : ResourceProvider
            where TCredentials : TokenCredentials
        {
#if __ANDROID__
            switch (browserType)
            {
                case AuthorizationInterface.Dedicated:
                    return new BrowserAuthorizerUI<TCredentials>(
                        new ProtocolOAuthCallbackListener<TCredentials>(
                            handler,
                            Platform.Current),
                        Platform.Current,
                        callbackUri);
                case AuthorizationInterface.Embedded:
                    return new WebViewAuthorizerUI<TCredentials>(
                        handler,
                        callbackUri);
                default:
                    throw new NotSupportedException();
            }
#elif __IOS__
            switch (browserType)
            {
                case AuthorizationInterface.Dedicated:
                    return new BrowserAuthorizerUI<TCredentials>(
                        new ProtocolOAuthCallbackListener<TCredentials>(
                            handler,
                            Platform.Current),
                        Platform.Current,
                        callbackUri);
                case AuthorizationInterface.Embedded:
                    return new UIWebViewAuthorizerUI<TCredentials>(
                        handler,
                        callbackUri);
                default:
                    throw new NotSupportedException();
            }
#elif WINDOWS_UWP
            switch (browserType)
            {
                case AuthorizationInterface.Dedicated:
                    return new BrowserAuthorizerUI<TCredentials>(
                        new ProtocolOAuthCallbackListener<TCredentials>(
                            handler,
                            Platform.Current),
                        Platform.Current,
                        callbackUri);
                case AuthorizationInterface.Embedded:
                    return new WebViewAuthorizerUI<TCredentials>(
                        handler,
                        callbackUri);
                default:
                    throw new NotSupportedException();
            }
#elif __WINDOWS__
            return new BrowserAuthorizerUI<TCredentials>(
                new HttpOAuthCallbackListener<TCredentials>(
                    new HttpServer(),
                    handler), 
                Platform.Current,
                callbackUri);
#else
            throw new NotSupportedException();
#endif
        }
    }
}
