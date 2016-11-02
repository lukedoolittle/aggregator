using System;
using System.Text;
using Foundations.HttpClient.Cryptography;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JsonWebTokenSigningTemplate
    {
        private readonly IJsonWebTokenSigningFactory _factory;

        public OAuth2JsonWebTokenSigningTemplate(
            IJsonWebTokenSigningFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _factory = factory;
        }

        public string CreateSignature(
            string signatureBase, 
            JsonWebTokenAlgorithm algorithm,
            string privateKey)
        {
            var signingAlgorithm = _factory.GetAlgorithm(algorithm);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signatureBytes = signingAlgorithm.SignText(
                signatureBaseBytes, 
                privateKey);

            return Convert.ToBase64String(signatureBytes);
        }
    }
}
