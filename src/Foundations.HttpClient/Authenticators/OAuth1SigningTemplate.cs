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
            var allParameters = new List<KeyValuePair<string, string>>(
                parameters);

            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.ConsumerKey, _consumerKey);
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.OAuthToken, _oauthToken);
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.Verifier, _verifier);
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.Callback, _callbackUrl?.ToString());
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.Timestamp, Timestamp);
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.Nonce, Nonce);
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.SignatureMethod, _signingAlgorithm.SignatureMethod);
            AddOAuth1Parameter(allParameters, OAuth1ParameterEnum.Version, Version);

            var elements = new List<string>
            {
                method.ToString(),
                url.ToString().UrlEncodeString(),
                allParameters.EncodeAndSortParameters().Concatenate("=", "&").UrlEncodeString()
            };

            return elements.Concatenate("&");
        }

        private static void AddOAuth1Parameter(
            ICollection<KeyValuePair<string, string>> parameters, 
            OAuth1ParameterEnum parameterKey,
            string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            parameters.Add(
                new KeyValuePair<string, string>(
                    parameterKey.EnumToString(),
                    value));
        }

        public string GenerateSignature(string signatureBase)
        {
            var key = $"{_consumerSecret.UrlEncodeString()}&{_oauthSecret.UrlEncodeString()}";
            var signature = _signingAlgorithm.SignText(
                Encoding.UTF8.GetBytes(signatureBase), 
                key);

            return Convert.ToBase64String(signature);
        }
    }
}

