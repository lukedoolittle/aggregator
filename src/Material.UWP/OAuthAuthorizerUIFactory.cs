using System;
using Material.Contracts;
using Material.Domain.Core;
using Material.Domain.Credentials;
using Material.Framework;
using Material.Framework.Enums;
using Material.View.WebAuthorization;
using Material.Workflow;

namespace Material.Workflow
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
                return new WebViewAuthorizerUI<TCredentials>(
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
