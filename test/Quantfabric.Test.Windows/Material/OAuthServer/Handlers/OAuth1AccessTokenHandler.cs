using System.Collections.Generic;
using System.Net;
using Foundations.Enums;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Serialization;
using Quantfabric.Test.Material.OAuthServer.Tokens;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1AccessTokenHandler : OAuth1RequestHandlerBase
    {
        private readonly ICredentialBuilder<OAuth1Credentials, OAuth1Request> _builder;

        public OAuth1AccessTokenHandler(
            string consumerKey,
            OAuth1SignatureVerifier verifier,
            IDictionary<string, List<OAuth1Token>> tokens,
            IIncommingMessageDeserializer deserializer,
            ICredentialBuilder<OAuth1Credentials, OAuth1Request> builder) :
                base(
                    consumerKey, 
                    verifier,
                    deserializer,
                    tokens)
        {
            _builder = builder;
        }

        public override void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            base.HandleRequest(request, response);

            var message = _deserializer
                .DeserializeMessage<OAuth1Request>(request);

            var credentials = _builder.BuildCredentials(message);

            var responseBody = new JsonSerializer().Serialize(credentials);

            response.WriteHead(HttpStatusCode.OK);
            response.WriteHead(HttpRequestHeader.ContentType, MediaType.Json);
            response.Write(responseBody);
            response.End();
        }
    }
}
