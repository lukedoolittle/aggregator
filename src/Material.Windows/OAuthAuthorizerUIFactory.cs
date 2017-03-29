using System;
using Foundations.Http;
using Material.Contracts;
using Material.Domain.Core;
using Material.Domain.Credentials;
using Material.Framework;
using Material.Framework.Enums;
using Material.View.WebAuthorization;

namespace Material.Workflow
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
            return new BrowserAuthorizerUI<TCredentials>(
                Platform.Current,
                new HttpServer(),
                handler,
                callbackUri,
                AuthorizationInterface.Dedicated,
                null,
                () => true);
        }
    }
}
