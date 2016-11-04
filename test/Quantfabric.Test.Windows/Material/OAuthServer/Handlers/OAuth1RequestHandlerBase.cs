using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.Http;
using Quantfabric.Test.Material.OAuth2Server;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Serialization;
using Quantfabric.Test.Material.OAuthServer.Tokens;

namespace Quantfabric.Test.Material.OAuthServer.Handlers
{
    public abstract class OAuth1RequestHandlerBase : IOAuthHandler
    {
        protected readonly IIncommingMessageDeserializer _deserializer;
        private readonly OAuth1SignatureVerifier _signatureVerifier;
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;
        private readonly string _consumerKey;
        private readonly string _version = "1.0";
        private readonly string _signatureMethod = "HMAC-SHA1";
        private readonly int _maximumTimeDifferenceInMinutes = 10;

        protected OAuth1RequestHandlerBase(
            string consumerKey, 
            OAuth1SignatureVerifier verifier,
            IIncommingMessageDeserializer deserializer, 
            IDictionary<string, List<OAuth1Token>> tokens)
        {
            _consumerKey = consumerKey;
            _deserializer = deserializer;
            _tokens = tokens;
            _signatureVerifier = verifier;
        }

        public virtual void HandleRequest(
            IncomingMessage request, 
            ServerResponse response)
        {
            var message = _deserializer
                .DeserializeMessage<OAuth1Request>(request);

            if (message.ConsumerKey != _consumerKey)
            {
                throw new Exception();
            }
            if (message.OAuthVersion != _version)
            {
                throw new Exception();
            }
            if (message.SignatureMethod != _signatureMethod)
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
                var timestamp = Convert
                    .ToInt64(message.Timestamp)
                    .FromUnixTimeSeconds();

                if ((DateTime.Now - timestamp).TotalMinutes > _maximumTimeDifferenceInMinutes)
                {
                    throw new Exception();
                }
            }

            if (_tokens != null && 
                message.OAuthToken != null && 
                _tokens.ContainsKey(message.OAuthToken))
            {
                _signatureVerifier.SetOAuthSecret(
                    _tokens[message.OAuthToken]
                        .FirstOrDefault()?
                        .OAuthSecret);
            }

            var parameters = new HttpValueCollection
            {
                HttpUtility.ParseQueryString(request.Uri.Query),
                HttpUtility.ParseQueryString(request.BodyAsString)
            };

            if (!_signatureVerifier.IsSignatureValid(
                request.Uri, 
                request.Method,
                parameters))
            {
                throw new Exception();
            }
        }
    }
}
