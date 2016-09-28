using System.Collections.Generic;
using Foundations.Cryptography.JsonWebToken;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1ProtectedResource : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _oauthToken;
        private readonly OAuthParameterTypeEnum _parameterHandling;

        public OAuth1ProtectedResource(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken,
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm = null)
        {
            _consumerKey = consumerKey;
            _oauthToken = oauthToken;

            var signer = signingAlgorithm ?? DigestSigningAlgorithm.Sha1Algorithm();
            _template = new OAuth1SigningTemplate(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret,
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
                    OAuth1ParameterEnum.SignatureMethod.EnumToString(),
                    OAuth1SigningTemplate.SIGNATURE_METHOD)
                .Parameter(
                    OAuth1ParameterEnum.Version.EnumToString(),
                    OAuth1SigningTemplate.VERSION)
                .Parameter(
                    OAuth1ParameterEnum.Signature.EnumToString(),
                    signature);
        }
    }
}
