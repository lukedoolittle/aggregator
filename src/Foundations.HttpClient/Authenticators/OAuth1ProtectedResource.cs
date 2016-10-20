using System;
using Foundations.Cryptography.DigitalSignature;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1ProtectedResource : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _oauthToken;
        private readonly string _signatureMethod;
        //TODO: why is this unused?
#pragma warning disable 169
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly HttpParameterType _parameterHandling;
#pragma warning restore 169

        public OAuth1ProtectedResource(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken,
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator)
        {
            if (signingAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(signingAlgorithm));
            }
            if (stringGenerator == null)
            {
                throw new ArgumentNullException(nameof(stringGenerator));
            }

            _consumerKey = consumerKey;
            _oauthToken = oauthToken;

            _signatureMethod = signingAlgorithm.SignatureMethod;
            _template = new OAuth1SigningTemplate(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret,
                signingAlgorithm,
                stringGenerator);
        }

        public OAuth1ProtectedResource(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret) : 
            this(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret, 
                DigestSigningAlgorithm.Sha1Algorithm(), 
                new CryptoStringGenerator())
        { }

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null)
            {
                throw new ArgumentNullException(nameof(requestBuilder));
            }

            var signatureBase = _template.ConcatenateElements(
                requestBuilder.Method,
                requestBuilder.Url,
                requestBuilder.QueryParameters);

            var signature = _template.GenerateSignature(signatureBase);

            requestBuilder
                .Parameter(
                    OAuth1ParameterEnum.Nonce,
                    _template.Nonce)
                .Parameter(
                    OAuth1ParameterEnum.Timestamp,
                    _template.Timestamp)
                .Parameter(
                    OAuth1ParameterEnum.ConsumerKey,
                    _consumerKey)
                .Parameter(
                    OAuth1ParameterEnum.OAuthToken,
                    _oauthToken)
                .Parameter(
                    OAuth1ParameterEnum.SignatureMethod,
                    _signatureMethod)
                .Parameter(
                    OAuth1ParameterEnum.Version,
                    _template.Version)
                .Parameter(
                    OAuth1ParameterEnum.Signature,
                    signature);
        }
    }
}
