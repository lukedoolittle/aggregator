using System;
using System.Text;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;

namespace Foundations.HttpClient.Authenticators
{
    public class JsonWebTokenSigningTemplate
    {
        private readonly IJsonWebTokenSigningFactory _factory;

        public JsonWebTokenSigningTemplate(
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
            CryptoKey privateKey)
        {
            var signingAlgorithm = _factory.GetSigningAlgorithm(algorithm);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signatureBytes = signingAlgorithm.SignText(
                signatureBaseBytes, 
                privateKey);

            return Convert.ToBase64String(signatureBytes);
        }
    }
}
