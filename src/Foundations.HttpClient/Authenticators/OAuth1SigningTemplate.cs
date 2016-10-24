using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
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
            DateTime timestamp,
            string nonce)
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
            if (nonce == null) throw new ArgumentNullException(nameof(nonce));

            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUrl = callbackUrl;
            Version = DEFAULT_VERSION;
            _signingAlgorithm = signingAlgorithm;

            Timestamp = ((int)timestamp.ToUnixTimeSeconds())
                .ToString(CultureInfo.InvariantCulture);
            Nonce = nonce;
        }

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            Uri callbackUrl,
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator) : 
                this(
                    consumerKey, 
                    consumerSecret, 
                    callbackUrl, 
                    signingAlgorithm, 
                    DateTime.UtcNow, 
                    stringGenerator?.CreateRandomString(
                        16,
                        CryptoStringType.LowercaseAlphanumeric))
        {}

        public OAuth1SigningTemplate(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            string verifier, 
            ISigningAlgorithm signingAlgorithm,
            DateTime timestamp,
            string nonce) :
                this(
                    consumerKey, 
                    consumerSecret, 
                    oauthToken, 
                    oauthSecret, 
                    signingAlgorithm,
                    timestamp, 
                    nonce)
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
            string verifier,
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator) :
                this(
                    consumerKey,
                    consumerSecret,
                    oauthToken,
                    oauthSecret,
                    verifier,
                    signingAlgorithm,
                    DateTime.UtcNow,
                    stringGenerator?.CreateRandomString(
                        16,
                        CryptoStringType.LowercaseAlphanumeric))
        {}

        public OAuth1SigningTemplate(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm,
            DateTime timestamp,
            string nonce) :
                this(
                    consumerKey,
                    consumerSecret,
                    null,
                    signingAlgorithm,
                    timestamp, 
                    nonce)
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
                            oauthToken,
                            oauthSecret,
                            signingAlgorithm,
                            DateTime.UtcNow,
                            stringGenerator?.CreateRandomString(
                                16,
                                CryptoStringType.LowercaseAlphanumeric))
        {}

        public string ConcatenateElements(
            HttpMethod method,
            Uri url,
            HttpValueCollection parameters)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var allParameters = new HttpValueCollection(parameters);

            AddOAuth1Parameter(allParameters, OAuth1Parameter.ConsumerKey, _consumerKey);
            AddOAuth1Parameter(allParameters, OAuth1Parameter.OAuthToken, _oauthToken);
            AddOAuth1Parameter(allParameters, OAuth1Parameter.Verifier, _verifier);
            AddOAuth1Parameter(allParameters, OAuth1Parameter.Callback, _callbackUrl?.ToString());
            AddOAuth1Parameter(allParameters, OAuth1Parameter.Timestamp, Timestamp);
            AddOAuth1Parameter(allParameters, OAuth1Parameter.Nonce, Nonce);
            AddOAuth1Parameter(allParameters, OAuth1Parameter.SignatureMethod, _signingAlgorithm.SignatureMethod);
            AddOAuth1Parameter(allParameters, OAuth1Parameter.Version, Version);

            var elements = new List<string>
            {
                method.ToString(),
                url.ToString().UrlEncodeString(),
                allParameters.EncodeAndSortParameters().Concatenate("=", "&").UrlEncodeString()
            };

            return elements.Concatenate("&");
        }

        private static void AddOAuth1Parameter(
            HttpValueCollection parameters, 
            OAuth1Parameter parameterKey,
            string value)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            parameters.Add(parameterKey.EnumToString(), value);
        }

        public string GenerateSignature(string signatureBase)
        {
            if (signatureBase == null) throw new ArgumentNullException(nameof(signatureBase));

            var key = StringExtensions.Concatenate(
                _consumerSecret.UrlEncodeString(),
                _oauthSecret.UrlEncodeString(),
                "&");

            var signature = _signingAlgorithm.SignText(
                Encoding.UTF8.GetBytes(signatureBase), 
                key);

            return Convert.ToBase64String(signature);
        }
    }
}

