using System;
using System.Net;
using Foundations.Enums;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.OAuthServer.Builders;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1RequestTokenHandler : OAuth1RequestHandlerBase
    {
        private readonly ICredentialBuilder<OAuth1Credentials, OAuth1Request> _builder;

        public OAuth1RequestTokenHandler(
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

            var message = new HtmlSerializer()
                .Deserialize<OAuth1Request>(
                    request.Uri.Query);

            var credentials = _builder.BuildCredentials(message);

            var responseBody = new JsonSerializer().Serialize(credentials);

            response.WriteHead(HttpStatusCode.OK);
            response.WriteHead(HttpRequestHeader.ContentType, MediaType.Json);
            response.Write(responseBody);
            response.End();
        }
    }
}
