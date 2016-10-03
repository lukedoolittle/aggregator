using Foundations.Cryptography.DigitalSignature;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth1ProtectedResource : IAuthenticator
    {
        private readonly OAuth1SigningTemplate _template;
        private readonly string _consumerKey;
        private readonly string _oauthToken;
        private readonly string _signatureMethod;
        private readonly OAuthParameterTypeEnum _parameterHandling;

        public OAuth1ProtectedResource(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken,
            string oauthSecret,
            ISigningAlgorithm signingAlgorithm = null,
            ICryptoStringGenerator stringGenerator = null)
        {
            _consumerKey = consumerKey;
            _oauthToken = oauthToken;

            var signer = signingAlgorithm ?? DigestSigningAlgorithm.Sha1Algorithm();
            var generator = stringGenerator ?? new CryptoStringGenerator();

            _signatureMethod = signer.SignatureMethod;
            _template = new OAuth1SigningTemplate(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret,
                signer,
                generator);
        }

        public void Authenticate(HttpRequest request)
        {
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
