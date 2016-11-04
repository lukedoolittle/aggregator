using System;
using System.Collections.Generic;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuth2Server;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Requests;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1AuthorizationHandler : IOAuthHandler
    {
        private readonly IRedirectUriBuilder<OAuth1Credentials> _redirector;
        private readonly ICredentialBuilder<OAuth1Credentials, OAuth1Request> _builder;
        private readonly Uri _redirectUriBase;

        public OAuth1AuthorizationHandler(
            Uri redirectUriBase,
            IRedirectUriBuilder<OAuth1Credentials> redirector,
            ICredentialBuilder<OAuth1Credentials, OAuth1Request> builder)
        {
            _redirectUriBase = redirectUriBase;
            _redirector = redirector;
            _builder = builder;
        }

        public void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            var message = new HtmlSerializer()
                .Deserialize<OAuth1Request>(
                    request.Uri.Query);

            var redirectUri = _redirector.BuildRedirectUri(
                _redirectUriBase,
                _builder.BuildCredentials(message),
                new Dictionary<string, string>());

            response.Redirect(redirectUri);
        }
    }
}
