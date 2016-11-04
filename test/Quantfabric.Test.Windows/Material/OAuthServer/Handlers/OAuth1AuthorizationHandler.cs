using System;
using Foundations.Extensions;
using Foundations.Http;
using Foundations.HttpClient.Serialization;
using Quantfabric.Test.Material.OAuth2Server;
using Quantfabric.Test.Material.OAuthServer.Requests;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public class OAuth1AuthorizationHandler : IOAuthHandler
    {
        private readonly Uri _redirectUriBase;
        private readonly string _consumerKey;
        private readonly string _oauthToken;
        private readonly string _version = "1.0";
        private readonly string _signatureMethod = "HMAC-SHA1";
        private readonly int _maximumTimeDifferenceInMinutes;

        public void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            var message = new HtmlSerializer()
                .Deserialize<OAuth1AuthorizationRequest>(
                    request.Uri.Query);

            if (message.ConsumerKey != _consumerKey)
            {
                throw new Exception();
            }
            if (message.OAuthToken != _oauthToken)
            {
                throw new Exception();
            }
            if (message.RedirectUri != _redirectUriBase.ToString())
            {
                throw new Exception();
            }
            if (string.IsNullOrEmpty(message.Nonce))
            {
                throw new Exception();
            }
            if (string.IsNullOrEmpty(message.Timestamp))
            {
                throw new Exception();
            }
            else
            {
                var timestampDifference =
                    DateTime.Now - Convert.ToInt64(message.Timestamp).FromUnixTimeMilliseconds();

                if (timestampDifference.TotalMinutes > _maximumTimeDifferenceInMinutes)
                {
                    throw new Exception();
                }
            }
        }
    }
}
