using System;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.View.WebAuthorization;
#if __WINDOWS__
using Foundations.Http;
#endif

namespace Material.OAuth
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
#if __WINDOWS__
            return new BrowserAuthorizerUI<TCredentials>(
                new HttpServer(),
                handler, 
                callbackUri,
                AuthorizationInterface.Dedicated,
                null,
                () => true);
#else
            if (browserType == AuthorizationInterface.Dedicated)
            {
                return new ProtocolAuthorizerUI<TCredentials>(
                    handler,
                    callbackUri,
                    browserType,
                    Framework.Platform.Current.RunOnMainThread,
                    () => Framework.Platform.Current.IsOnline);
            }
            else if (browserType == AuthorizationInterface.Embedded)
            {
#if __ANDROID__
                return new WebViewAuthorizerUI<TCredentials>(
                    handler,
                    callbackUri,
                    browserType,
                    Framework.Platform.Current.RunOnMainThread,
                    () => Framework.Platform.Current.IsOnline);
#elif __IOS__
                return new UIWebViewAuthorizerUI<TCredentials>(
                    handler,
                    callbackUri,
                    browserType,
                    Framework.Platform.Current.RunOnMainThread,
                    () => Framework.Platform.Current.IsOnline);
#elif WINDOWS_UWP
                return new WebViewAuthorizerUI<TCredentials>(
                    handler,
                    callbackUri,
                    browserType,
                    Framework.Platform.Current.RunOnMainThread,
                    () => Framework.Platform.Current.IsOnline);
#endif
            }
            else
            {
                throw new NotSupportedException();
            }
#endif
        }
    }
}
