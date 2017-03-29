using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Canonicalizers;
using Material.HttpClient.Cryptography.Algorithms;
using Material.HttpClient.Cryptography.Enums;
using Material.HttpClient.Cryptography.Keys;

namespace Material.HttpClient.Authenticators
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

            var signature = _signingAlgorithm.SignMessage(
                signatureBase,
                new HashKey(key, StringEncoding.Utf8));

            builder.Parameter(
                OAuth1Parameter.Signature.EnumToString(),
                signature.ToBase64String());
        }
    }
}
