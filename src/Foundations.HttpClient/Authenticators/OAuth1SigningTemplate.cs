using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Foundations.Extensions;
using Foundations.Cryptography;
using Foundations.Cryptography.JsonWebToken;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1SigningTemplate
    {
        public const string SIGNATURE_METHOD = "HMAC-SHA1";
        public const string VERSION = "1.0";

        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _oauthToken;
        private readonly string _oauthSecret;
        private readonly string _verifier;
        private readonly ISigningAlgorithm _signingAlgorithm;

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            ISigningAlgorithm signingAlgorithm)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }
            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }
            if (signingAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(signingAlgorithm));
            }
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _signingAlgorithm = signingAlgorithm;
        }

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            string verifier, 
            ISigningAlgorithm signingAlgorithm) :
            this(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret, 
                signingAlgorithm)
        {
            if (string.IsNullOrEmpty(verifier))
            {
                throw new ArgumentNullException(nameof(verifier));
            }

            _verifier = verifier;
        }

        public OAuth1SigningTemplate(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm) :
                this(
                    consumerKey,
                    consumerSecret, 
                    signingAlgorithm)
        {
            if (string.IsNullOrEmpty(oauthToken))
            {
                throw new ArgumentNullException(nameof(oauthToken));
            }
            if (string.IsNullOrEmpty(oauthSecret))
            {
                throw new ArgumentNullException(nameof(oauthSecret));
            }

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
                    Security.Create16CharacterCryptographicallyStrongString())
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
            if (_verifier != null)
            {
                allParameters.Add(
                    new KeyValuePair<string, string>(
                        OAuth1ParameterEnum.Verifier.EnumToString(),
                        _verifier));
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
            var signature = _signingAlgorithm.SignText(
                Encoding.UTF8.GetBytes(signatureBase), 
                key);

            return Convert.ToBase64String(signature);
        }
    }
}

