using System;
using Foundations.Http;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public IOAuthAuthorizerUI<TCredentials> GetAuthorizer<TService, TCredentials>(
            AuthorizationInterface browserType,
            IOAuthCallbackHandler<TCredentials> handler, 
            Uri callbackUri) 
            where TService : ResourceProvider 
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
