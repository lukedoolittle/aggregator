using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1RequestSigningAlgorithm : IRequestSigningAlgorithm
    {
        private readonly string _consumerSecret;
        private readonly string _oauthSecret;
        private readonly ISigningAlgorithm _signingAlgorithm;

        public OAuth1RequestSigningAlgorithm(
            string consumerSecret, 
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm)
        {
            _consumerSecret = consumerSecret;
            _oauthSecret = oauthSecret;
            _signingAlgorithm = signingAlgorithm;
        }

        public OAuth1RequestSigningAlgorithm(
            string consumerSecret,
            ISigningAlgorithm signingAlgorithm) : 
                this(
                    consumerSecret, 
                    null, 
                    signingAlgorithm)
        { }

        public void SignRequest(HttpRequestBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var signatureBase = ConcatenateElements(
                builder.Method, 
                builder.Url, 
                builder.QueryParameters);

            var signature = GenerateSignature(signatureBase);

            builder.Parameter(
                OAuth1Parameter.Signature.EnumToString(), 
                signature);
        }

        private static string ConcatenateElements(
            HttpMethod method,
            Uri url,
            HttpValueCollection parameters)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var elements = new List<string>
            {
                method.ToString(),
                url.ToString().UrlEncodeString(),
                new HttpValueCollection(parameters)
                    .EncodeAndSortParameters()
                    .Concatenate("=", "&")
                    .UrlEncodeString()
            };

            return elements.Concatenate("&");
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
                new HashKey(key));

            return Convert.ToBase64String(signature);
        }
    }
}
