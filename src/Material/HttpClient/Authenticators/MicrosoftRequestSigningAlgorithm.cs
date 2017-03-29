using System;
using System.Net;
using Material.Framework.Extensions;
using Material.HttpClient.Canonicalizers;
using Material.HttpClient.Cryptography.Algorithms;
using Material.HttpClient.Cryptography.Enums;
using Material.HttpClient.Cryptography.Keys;

namespace Material.HttpClient.Authenticators
{
    public class MicrosoftRequestSigningAlgorithm : IRequestSigningAlgorithm
    {
        private readonly string _accountName;
        private readonly string _accountKey;
        private readonly string _accountKeyType;
        private readonly ISigningAlgorithm _signingAlgorithm;
        private readonly IHttpRequestCanonicalizer _canonicalizer;

        public MicrosoftRequestSigningAlgorithm(
            string accountName,
            string accountKey,
            string accountKeyType,
            ISigningAlgorithm signingAlgorithm, 
            IHttpRequestCanonicalizer canonicalizer)
        {
            _signingAlgorithm = signingAlgorithm;
            _canonicalizer = canonicalizer;
            _accountKeyType = accountKeyType;
            _accountKey = accountKey;
            _accountName = accountName;
        }

        public void SignRequest(HttpRequestBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var message = _canonicalizer
                .CanonicalizeHttpRequest(
                    builder);

            var signature = _signingAlgorithm.SignMessage(
                message,
                new HashKey(
                    _accountKey, 
                    StringEncoding.Base64));

            var nameAndSignature = StringExtensions.Concatenate(
                _accountName,
                signature.ToBase64String(),
                ":");

            var authorizationHeader = StringExtensions.Concatenate(
                _accountKeyType, 
                nameAndSignature, 
                " ");

            builder.Header(
                HttpRequestHeader.Authorization,
                authorizationHeader);
        }
    }
}
