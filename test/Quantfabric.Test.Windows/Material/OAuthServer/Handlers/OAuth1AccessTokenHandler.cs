using System;
using Foundations.Http;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.OAuthServer.Builders;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1AccessTokenHandler : OAuth1RequestHandlerBase
    {
        private readonly ICredentialBuilder<OAuth1Credentials, OAuth1Request> _builder;

        public OAuth1AccessTokenHandler(
            string consumerKey,
            Uri redirectUriBase, 
            ICredentialBuilder<OAuth1Credentials, OAuth1Request> builder) :
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

            //TODO: basically the same thing here as we did in request token although
            //check that the oauth token and verifier are correct

            throw new NotImplementedException("Handle access token request");
        }

    }
}
