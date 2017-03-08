using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuth2Server;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Tokens;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1AuthorizationHandler : IOAuthHandler
    {
        private readonly IRedirectUriBuilder<OAuth1Credentials> _redirector;
        private readonly ICredentialBuilder<OAuth1Credentials, OAuth1Request> _builder;
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;

        public OAuth1AuthorizationHandler(
            IRedirectUriBuilder<OAuth1Credentials> redirector,
            ICredentialBuilder<OAuth1Credentials, OAuth1Request> builder,
            IDictionary<string, List<OAuth1Token>> tokens)
        {
            _redirector = redirector;
            _builder = builder;
            _tokens = tokens;
        }

        public void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            var message = new HtmlSerializer()
                .Deserialize<OAuth1Request>(
                    request.Uri.Query);

            var token = _tokens[message.OAuthToken].FirstOrDefault();

            if (token == null)
            {
                throw new Exception("Couldn't find token for given OAuthToken");
            }

            var redirectUri = _redirector.BuildRedirectUri(
                new Uri(token.CallbackUri),
                _builder.BuildCredentials(message),
                new Dictionary<string, string>());

            response.Redirect(redirectUri);
        }
    }
}
