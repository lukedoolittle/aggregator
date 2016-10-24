using System;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1AccessToken : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _oauthToken;
        private readonly string _verifier;
        private readonly string _signatureMethod;

        public OAuth1AccessToken(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            string verifier,
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
            _verifier = verifier;

            _signatureMethod = signingAlgorithm.SignatureMethod;
            _template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                verifier,
                signingAlgorithm,
                stringGenerator);
        }

        public OAuth1AccessToken(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            string verifier) :
            this(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret, 
                verifier, 
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
                    OAuth1Parameter.Nonce,
                    _template.Nonce)
                .Parameter(
                    OAuth1Parameter.Timestamp,
                    _template.Timestamp)
                .Parameter(
                    OAuth1Parameter.ConsumerKey,
                    _consumerKey)
                .Parameter(
                    OAuth1Parameter.OAuthToken,
                    _oauthToken)
                .Parameter(
                    OAuth1Parameter.Verifier,
                    _verifier)
                .Parameter(
                    OAuth1Parameter.SignatureMethod,
                    _signatureMethod)
                .Parameter(
                    OAuth1Parameter.Version,
                    _template.Version)
                .Parameter(
                    OAuth1Parameter.Signature,
                    signature);
        }
    }
}
