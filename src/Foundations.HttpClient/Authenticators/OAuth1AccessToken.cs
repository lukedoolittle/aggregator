using System.Collections.Generic;
using Foundations.Cryptography.JsonWebToken;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1AccessToken : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _oauthToken;
        private readonly string _verifier;

        public OAuth1AccessToken(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            string verifier,
            ISigningAlgorithm signingAlgorithm = null)
        {
            _consumerKey = consumerKey;
            _oauthToken = oauthToken;
            _verifier = verifier;

            var signer = signingAlgorithm ?? DigestSigningAlgorithm.Sha1Algorithm();
            _template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                verifier,
                signer);
        }

        public void Authenticate(HttpRequest request)
        {
            var nonceAndTimestampParameters = _template.CreateNonceAndTimestamp();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.AddRange(nonceAndTimestampParameters);
            parameters.AddRange(request.QueryParameters);

            var signatureBase = _template.ConcatenateElements(
                request.Method,
                request.Url,
                parameters);

            var signature = _template.GenerateSignature(signatureBase);

            request
                .Parameters(nonceAndTimestampParameters)
                .Parameter(
                    OAuth1ParameterEnum.ConsumerKey.EnumToString(),
                    _consumerKey)
                .Parameter(
                    OAuth1ParameterEnum.OAuthToken.EnumToString(),
                    _oauthToken)
                .Parameter(
                    OAuth1ParameterEnum.Verifier.EnumToString(),
                    _verifier)
                .Parameter(
                    OAuth1ParameterEnum.SignatureMethod.EnumToString(),
                    _template.SignatureMethod)
                .Parameter(
                    OAuth1ParameterEnum.Version.EnumToString(),
                    _template.Version)
                .Parameter(
                    OAuth1ParameterEnum.Signature.EnumToString(),
                    signature);
        }
    }
}
