using System;
using System.Collections.Generic;
using System.Net.Http;
using Foundations.Extensions;
using Foundations.Cryptography;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1SigningTemplate
    {
        private const int NONCE_LENGTH = 16;
        private const string SIGNATURE_METHOD = "HMAC-SHA1";
        private const string VERSION = "1.0";

        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _oauthToken;
        private readonly string _oauthSecret;

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _oauthToken = oauthToken;
            _oauthSecret = oauthSecret;
        }

        private string GetTimestamp()
        {
            return ((int)DateTime.UtcNow.ToUnixTimeSeconds()).ToString();
        }

        public IEnumerable<KeyValuePair<string, string>> CreateNonceAndTimestamp()
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.Timestamp.EnumToString(),
                    GetTimestamp()),
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.Nonce.EnumToString(),
                    Security.GetNonce(NONCE_LENGTH))
            };

            return parameters;
        }

        public string ConcatenateElements(
            HttpMethod method,
            Uri url,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var allParameters = new List<KeyValuePair<string, string>>();
            allParameters.AddRange(parameters);

            if (_consumerKey != null)
            {
                allParameters.Add(
                    new KeyValuePair<string, string>(
                        OAuth1ParameterEnum.ConsumerKey.EnumToString(), 
                        _consumerKey));
            }
            if (_oauthToken != null)
            {
                allParameters.Add(
                    new KeyValuePair<string, string>(
                        OAuth1ParameterEnum.OAuthToken.EnumToString(),
                        _oauthToken));
            }

            allParameters.Add(
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.SignatureMethod.EnumToString(),
                    SIGNATURE_METHOD));
            allParameters.Add(
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.Version.EnumToString(),
                    VERSION));

            var elements = new List<string>
            {
                method.ToString(),
                url.UrlEncodeRelaxed(),
                allParameters.Normalize().Concatenate("=", "&").UrlEncodeRelaxed()
            };

            return elements.Concatenate("&");
        }

        public string GenerateSignature(string signatureBase)
        {
            var key = $"{_consumerSecret.UrlEncodeRelaxed()}&{_oauthSecret.UrlEncodeRelaxed()}";
            var signature = Security.Sha1Hash(
                key, 
                signatureBase);

            return signature;
        }
    }
}
