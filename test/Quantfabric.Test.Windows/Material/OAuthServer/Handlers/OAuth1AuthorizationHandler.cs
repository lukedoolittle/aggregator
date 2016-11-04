using System;
using Foundations.Http;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.OAuthServer.Builders;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1AuthorizationHandler : OAuth1RequestHandlerBase
    {
        private readonly IRedirectUriBuilder<OAuth1Credentials> _builder;

        public OAuth1AuthorizationHandler(
            string consumerKey,
            Uri redirectUriBase,
            IRedirectUriBuilder<OAuth1Credentials> builder) : 
                base(
                    consumerKey, 
                    redirectUriBase)
        {
            _builder = builder;
        }

        public override void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            base.HandleRequest(request, response);

            //TODO: a hybrid of the request token handler and the OAuth2AuthorizationHandler

            throw new NotImplementedException("Handle authorization request");
        }
    }
}
