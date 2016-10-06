using System;
using Foundations.Cryptography.DigitalSignature;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1RequestToken : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _callbackUrl;
        private readonly string _signatureMethod;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1RequestToken(
            string consumerKey, 
            string consumerSecret, 
            string callbackUrl,
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1RequestToken(
            string consumerKey,
            string consumerSecret,
            string callbackUrl) : 
                this(
                    consumerKey, 
                    consumerSecret,
                    callbackUrl, 
                    DigestSigningAlgorithm.Sha1Algorithm(), 
                    new CryptoStringGenerator())
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OAuth1RequestToken(
            string consumerKey,
            string callbackUrl,
            OAuth1SigningTemplate template,
            string signatureMethod)
        {
            _consumerKey = consumerKey;
            _callbackUrl = callbackUrl;
            _template = template;
            _signatureMethod = signatureMethod;
        }

        public void Authenticate(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var signatureBase = _template.ConcatenateElements(
                request.Method, 
                request.Url,
                request.QueryParameters);

            var signature = _template.GenerateSignature(signatureBase);

            request
                .Parameter(
                    OAuth1ParameterEnum.Nonce,
                    _template.Nonce)
                .Parameter(
                    OAuth1ParameterEnum.Timestamp, 
                    _template.Timestamp)
                .Parameter(
                    OAuth1ParameterEnum.Callback,
                    _callbackUrl)
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
