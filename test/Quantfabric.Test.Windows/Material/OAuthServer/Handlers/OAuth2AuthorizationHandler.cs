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
    public class OAuth2AuthorizationHandler : IOAuthHandler
    {
        private readonly Uri _redirectUriBase;
        private readonly string _clientId;
        private readonly IRedirectUriBuilder<OAuth2Credentials> _redirector;
        private readonly ICredentialBuilder<OAuth2Credentials, OAuth2AuthorizationRequest> _builder;
        private readonly bool _requiresScope;

        public OAuth2AuthorizationHandler(
            string clientId, 
            Uri redirectUriBase, 
            IRedirectUriBuilder<OAuth2Credentials> redirector,
            ICredentialBuilder<OAuth2Credentials, OAuth2AuthorizationRequest> builder,
            bool requiresScope)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (redirectUriBase == null)
            {
                throw new ArgumentNullException(nameof(redirectUriBase));
            }
            if (redirector == null)
            {
                throw new ArgumentNullException(nameof(redirector));
            }
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            _redirectUriBase = redirectUriBase;
            _redirector = redirector;
            _builder = builder;
            _clientId = clientId;
            _requiresScope = requiresScope;
        }

        public void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            var message = new HtmlSerializer()
                .Deserialize<OAuth2AuthorizationRequest>(
                    request.Uri.Query);

            if (message.ClientId != _clientId)
            {
                throw new Exception();
            }
            if (message.RedirectUri != _redirectUriBase.ToString())
            {
                throw new Exception();
            }
            if (_requiresScope && string.IsNullOrEmpty(message.Scope))
            {
                throw new Exception();
            }

            var redirectUri = _redirector.BuildRedirectUri(
                _redirectUriBase,
                _builder.BuildCredentials(message), 
                new Dictionary<string, string> { { "state", message.State}});

            response.Redirect(redirectUri);
        }
    }
}
