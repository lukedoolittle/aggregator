using System;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.View.WebAuthorization;

namespace Material.OAuth
{
    public class OAuthAuthorizerUIFactory : IOAuthAuthorizerUIFactory
    {
        public IOAuthAuthorizerUI<TCredentials> GetAuthorizer<TResourceProvider, TCredentials>(
            AuthorizationInterface browserType,
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri)
            where TResourceProvider : ResourceProvider
            where TCredentials : TokenCredentials
        {
            if (browserType == AuthorizationInterface.Dedicated)
            {
                return new ProtocolAuthorizerUI<TCredentials>(
                    Platform.Current,
                    Platform.Current,
                    handler,
                    callbackUri,
                    browserType,
                    Platform.Current.RunOnMainThread,
                    () => Platform.Current.IsOnline);
            }
            else if (browserType == AuthorizationInterface.Embedded)
            {
                return new UIWebViewAuthorizerUI<TCredentials>(
                    handler,
                    callbackUri,
                    browserType,
                    Platform.Current.RunOnMainThread,
                    () => Platform.Current.IsOnline);
            }
            else if (browserType == AuthorizationInterface.SecureEmbedded)
            {
                return new SafariWebViewAuthorizerUI<TCredentials>(
                    Platform.Current,
                    Platform.Current.Context,
                    handler,
                    callbackUri,
                    browserType,
                    Platform.Current.RunOnMainThread,
                    () => Platform.Current.IsOnline);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}