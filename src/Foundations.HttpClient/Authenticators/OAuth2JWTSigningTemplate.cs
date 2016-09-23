using System;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JWTSigningTemplate
    {
        private readonly JsonWebToken _token;

        public OAuth2JWTSigningTemplate(JsonWebToken token)
        {
            _token = token;

            if (token.Header.Algorithm != "RS256")
            {
                throw new NotSupportedException();
            }
        }

        public string CreateSignatureBase()
        {
            var header = SerializedHeader();
            var claims = SerializedClaims();

            return $"{header.ToUrlEncodedBase64String()}.{claims.ToUrlEncodedBase64String()}";
        }

        public string CreateSignature(
            string signatureBase, 
            string privateKey)
        {
            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var rs256 = Cryptography.Security.RS256(
                signatureBaseBytes, 
                privateKey);

            return rs256.ToUrlEncodedBase64String();
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
