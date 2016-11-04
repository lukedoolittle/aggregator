using System;
using System.Collections.Specialized;
using System.Net.Http;
using DotNetOpenAuth.OAuth;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Quantfabric.Test.Material.OAuthServer
{
    public class OAuth1SignatureVerifier : OAuth1HmacSha1HttpMessageHandler
    {
        private readonly string _consumerSecret;
        private string _oauthSecret;

        public OAuth1SignatureVerifier(string consumerSecret)
        {
            _consumerSecret = consumerSecret;
        }

        public void SetOAuthSecret(string oauthSecret)
        {
            _oauthSecret = oauthSecret;
        }

        public bool IsSignatureValid(
            Uri requestUri,
            HttpMethod requestMethod,
            HttpValueCollection parameters)
        {
            var expectedSignature = parameters[OAuth1Parameter.Signature.EnumToString()];
            parameters.Remove(OAuth1Parameter.Signature.EnumToString());

            ConsumerSecret = _consumerSecret;
            AccessTokenSecret = _oauthSecret;

            var signatureParameters = new NameValueCollection();
            foreach (var item in parameters)
            {
                signatureParameters.Add(item.Key, item.Value);
            }

            var builder = new UriBuilder(requestUri)
            {
                Fragment = string.Empty,
                Query = string.Empty
            };

            var request = new HttpRequestMessage(
                requestMethod,
                builder.Uri);

            var actualSignature = GetSignature(request, signatureParameters);

            return expectedSignature == actualSignature;
        }
    }
}
