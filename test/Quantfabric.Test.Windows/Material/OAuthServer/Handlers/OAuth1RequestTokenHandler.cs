using System;
using System.Net;
using Foundations.Enums;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Serialization;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1RequestTokenHandler : OAuth1RequestHandlerBase
    {
        private readonly IIncommingMessageDeserializer _deserializer;
        private readonly ICredentialBuilder<OAuth1Credentials, OAuth1Request> _builder;
        private readonly Uri _redirectUriBase;

        public OAuth1RequestTokenHandler(
            string consumerKey,
            Uri redirectUriBase,
            OAuth1SignatureVerifier verifier,
            IIncommingMessageDeserializer deserializer,
            ICredentialBuilder<OAuth1Credentials, OAuth1Request> builder) : 
                base(
                    consumerKey,
                    verifier, 
                    deserializer,
                    null)
        {
            _redirectUriBase = redirectUriBase;
            _builder = builder;
            _deserializer = deserializer;
        }

        public override void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            base.HandleRequest(request, response);

            var message = _deserializer
                .DeserializeMessage<OAuth1Request>(request);

            if (message.RedirectUri != _redirectUriBase.ToString())
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
