using System;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Canonicalizers;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1RequestSigningAlgorithm : IRequestSigningAlgorithm
    {
        private readonly string _consumerSecret;
        private readonly string _oauthSecret;
        private readonly ISigningAlgorithm _signingAlgorithm;
        private readonly IHttpRequestCanonicalizer _canonicalizer;

        public OAuth1RequestSigningAlgorithm(
            string consumerSecret, 
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm, 
            IHttpRequestCanonicalizer canonicalizer)
        {
            _consumerSecret = consumerSecret;
            _oauthSecret = oauthSecret;
            _signingAlgorithm = signingAlgorithm;
            _canonicalizer = canonicalizer;
        }

        public OAuth1RequestSigningAlgorithm(
            string consumerSecret,
            ISigningAlgorithm signingAlgorithm, 
            IHttpRequestCanonicalizer canonicalizer) : 
                this(
                    consumerSecret, 
                    null, 
                    signingAlgorithm, 
                    canonicalizer)
        { }

        public void SignRequest(HttpRequestBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var signatureBase = _canonicalizer
                .CanonicalizeHttpRequest(
                    builder);

            var key = StringExtensions.Concatenate(
                _consumerSecret.UrlEncodeString(),
                _oauthSecret.UrlEncodeString(),
                "&");

            var signature = _signingAlgorithm.SignText(
                Encoding.UTF8.GetBytes(signatureBase),
                new HashKey(key, StringEncoding.Utf8));

            builder.Parameter(
                OAuth1Parameter.Signature.EnumToString(),
                Convert.ToBase64String(signature));
        }
    }
}
