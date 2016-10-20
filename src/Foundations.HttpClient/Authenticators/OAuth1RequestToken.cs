using System;
using Foundations.Cryptography.DigitalSignature;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1RequestToken : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly Uri _callbackUrl;
        private readonly string _signatureMethod;

        public OAuth1RequestToken(
            string consumerKey, 
            string consumerSecret, 
            Uri callbackUrl,
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
            _callbackUrl = callbackUrl;

            _signatureMethod = signingAlgorithm.SignatureMethod;
            _template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                callbackUrl,
                signingAlgorithm,
                stringGenerator);
        }

        public OAuth1RequestToken(
            string consumerKey,
            string consumerSecret,
            Uri callbackUrl) : 
                this(
                    consumerKey, 
                    consumerSecret,
                    callbackUrl, 
                    DigestSigningAlgorithm.Sha1Algorithm(), 
                    new CryptoStringGenerator())
        { }

        public OAuth1RequestToken(
            string consumerKey,
            Uri callbackUrl,
            OAuth1SigningTemplate template,
            string signatureMethod)
        {
            _consumerKey = consumerKey;
            _callbackUrl = callbackUrl;
            _template = template;
            _signatureMethod = signatureMethod;
        }

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
                    OAuth1ParameterEnum.Callback,
                    _callbackUrl.ToString())
                .Parameter(
                    OAuth1ParameterEnum.ConsumerKey,
                    _consumerKey)
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
