using System;
using System.Net;
using Foundations.Enums;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuth2Server;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Requests;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth2AccessTokenHandler : IOAuthHandler
    {
        private readonly string _clientId;
        private readonly ICredentialBuilder<OAuth2Credentials, OAuth2TokenRequest> _builder;

        public OAuth2AccessTokenHandler(
            string clientId, 
            ICredentialBuilder<OAuth2Credentials, OAuth2TokenRequest> builder)
        {
            _clientId = clientId;
            _builder = builder;
        }

        public void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            var message = new HtmlSerializer()
                .Deserialize<OAuth2TokenRequest>(
                    request.BodyAsString);

            if (message.ClientId != _clientId)
            {
                throw new Exception();
            }

            var credentials = _builder.BuildCredentials(message);

            var responseBody = new JsonSerializer().Serialize(credentials);

            response.WriteHead(HttpStatusCode.OK);
            response.WriteHead(HttpRequestHeader.ContentType, MediaType.Json);
            response.Write(responseBody);
            response.End();
        }
    }
}
