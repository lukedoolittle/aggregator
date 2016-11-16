using System;
using System.Text;
using Foundations.Extensions;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string CreateSignatureBase(
            string header, 
            string claims)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            if (claims == null) throw new ArgumentNullException(nameof(claims));

            var headerBytes = Encoding.UTF8.GetBytes(header);
            var headerEncoded = Convert.ToBase64String(headerBytes);

            var claimsBytes = Encoding.UTF8.GetBytes(claims.Replace("\\", ""));
            var claimsEncoded = Convert.ToBase64String(claimsBytes);

            return StringExtensions.Concatenate(
                headerEncoded,
                claimsEncoded,
                ".");
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
