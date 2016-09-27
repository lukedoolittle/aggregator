using System;
using System.Text;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JWTSigningTemplate
    {
        private readonly JsonWebToken _token;

        public OAuth2JWTSigningTemplate(JsonWebToken token)
        {
            _token = token;
        }

        public string CreateSignatureBase()
        {
            var header = SerializedHeader();
            var headerBytes = Encoding.UTF8.GetBytes(header);
            var headerEncoded = Convert.ToBase64String(headerBytes);

            var claims = SerializedClaims();
            var claimsBytes = Encoding.UTF8.GetBytes(claims);
            var claimsEncoded = Convert.ToBase64String(claimsBytes);

            return $"{headerEncoded}.{claimsEncoded}";
        }

        public string CreateSignature(
            string signatureBase, 
            string privateKey,
            IJWTSigningFactory factory)
        {
            var signingAlgorithm = factory.GetAlgorithm(_token.Header.Algorithm);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signatureBytes = signingAlgorithm.SignText(
                signatureBaseBytes, 
                privateKey);

            return Convert.ToBase64String(signatureBytes);
        }

        public string CreateJsonWebToken(string signature)
        {
            return $"{CreateSignatureBase()}.{signature}";
        }

        private string SerializedHeader()
        {
            var serializer = new JsonSerializer();

            var header = serializer.Serialize(_token.Header);

            return header;
        }

        private string SerializedClaims()
        {
            var serializer = new JsonSerializer();

            var claims = serializer.Serialize(_token.Claims);

            //TODO: is there a more elegant way to remove escapes??
            return claims.Replace("\\", "");
        }
    }
}
