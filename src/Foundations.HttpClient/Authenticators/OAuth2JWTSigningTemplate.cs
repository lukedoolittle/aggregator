using System;
using System.Text;
using Foundations.Cryptography.JsonWebToken;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JwtSigningTemplate
    {
        private readonly JsonWebToken _token;
        private readonly IJwtSigningFactory _factory;

        public OAuth2JwtSigningTemplate(
            JsonWebToken token, 
            IJwtSigningFactory factory)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _token = token;
            _factory = factory;
        }

        public string CreateSignatureBase()
        {
            var serializer = new JsonSerializer();

            var header = serializer.Serialize(_token.Header);
            var headerBytes = Encoding.UTF8.GetBytes(header);
            var headerEncoded = Convert.ToBase64String(headerBytes);

            //TODO: is there a more elegant way to remove escapes??
            var claims = serializer.Serialize(_token.Claims).Replace("\\", ""); ;
            var claimsBytes = Encoding.UTF8.GetBytes(claims);
            var claimsEncoded = Convert.ToBase64String(claimsBytes);

            return $"{headerEncoded}.{claimsEncoded}";
        }

        public string CreateSignature(
            string signatureBase, 
            string privateKey)
        {
            var signingAlgorithm = _factory.GetAlgorithm(_token.Header.Algorithm);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signatureBytes = signingAlgorithm.SignText(
                signatureBaseBytes, 
                privateKey);

            return Convert.ToBase64String(signatureBytes);
        }
    }
}
