using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using Foundations.Extensions;
using Foundations.Cryptography;
using Foundations.Cryptography.DigitalSignature;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1SigningTemplate
    {
        public string Version { get; }
        public string Nonce { get; }
        public string Timestamp { get; }

        private const string DEFAULT_VERSION = "1.0";

        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _oauthToken;
        private readonly string _oauthSecret;
        private readonly Uri _callbackUrl;
        private readonly string _verifier;
        private readonly ISigningAlgorithm _signingAlgorithm;

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            Uri callbackUrl,
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator)
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
            if (stringGenerator == null)
            {
                throw new ArgumentNullException(nameof(stringGenerator));
            }

            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUrl = callbackUrl;
            Version = DEFAULT_VERSION;
            _signingAlgorithm = signingAlgorithm;

            Timestamp = ((int)DateTime.UtcNow.ToUnixTimeSeconds())
                .ToString(CultureInfo.InvariantCulture);
            Nonce = stringGenerator.CreateRandomString(
                16, 
                CryptoStringType.LowercaseAlphanumeric);
        }

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            string verifier, 
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator) :
            this(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret, 
                signingAlgorithm,
                stringGenerator)
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
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator) :
                this(
                    consumerKey,
                    consumerSecret,
                    null,
                    signingAlgorithm,
                    stringGenerator)
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

        public string ConcatenateElements(
            HttpMethod method,
            Uri url,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var allParameters = new List<KeyValuePair<string, string>>();
            allParameters.AddRange(parameters);

            //Request specific parameters

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
            if (_callbackUrl != null)
            {
                allParameters.Add(
                    new KeyValuePair<string, string>(
                        OAuth1ParameterEnum.Callback.EnumToString(),
                        _callbackUrl.ToString()));
            }

            //Required parameters for any request

            allParameters.Add(
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.Timestamp.EnumToString(),
                    Timestamp));

            allParameters.Add(
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.Nonce.EnumToString(),
                    Nonce));

            allParameters.Add(
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.SignatureMethod.EnumToString(),
                    _signingAlgorithm.SignatureMethod));

            allParameters.Add(
                new KeyValuePair<string, string>(
                    OAuth1ParameterEnum.Version.EnumToString(),
                    Version));

            var elements = new List<string>
            {
                method.ToString(),
                url.ToString().UrlEncodeRelaxed(),
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

