using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1RequestToken : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _callbackUrl;


        public OAuth1RequestToken(
            string consumerKey, 
            string consumerSecret, 
            string callbackUrl)
        {
            _consumerKey = consumerKey;
            _callbackUrl = callbackUrl;
            _template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret);
        }

        public void Authenticate(HttpRequest request)
        {
            var nonceAndTimestampParameters = _template.CreateNonceAndTimestamp();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>(
                OAuth1ParameterEnum.Callback.EnumToString(), 
                _callbackUrl));
            parameters.AddRange(nonceAndTimestampParameters);
            parameters.AddRange(request.QueryParameters);

            var signatureBase = _template.ConcatenateElements(
                request.Method, 
                request.Url, 
                parameters);

            var signature = _template.GenerateSignature(signatureBase);

            request
                .Parameters(nonceAndTimestampParameters)
                .Parameter(
                    OAuth1ParameterEnum.Callback.EnumToString(),
                    _callbackUrl)
                .Parameter(
                    OAuth1ParameterEnum.ConsumerKey.EnumToString(),
                    _consumerKey)
                .Parameter(
                    OAuth1ParameterEnum.SignatureMethod.EnumToString(),
                    "HMAC-SHA1")
                .Parameter(
                    OAuth1ParameterEnum.Version.EnumToString(),
                    "1.0")
                .Parameter(
                    OAuth1ParameterEnum.Signature.EnumToString(),
                    signature);
        }
    }
}
