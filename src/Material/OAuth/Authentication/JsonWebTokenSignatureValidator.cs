using System;
using System.Text;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Keys;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class JsonWebTokenSignatureValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly IJsonWebTokenSigningFactory _factory;
        private readonly CryptoKey _key;

        public JsonWebTokenSignatureValidator(
            CryptoKey key,
            IJsonWebTokenSigningFactory factory)
        {
            _factory = factory;
            _key = key;
        }

        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var verifier = _factory.GetVerificationAlgorithm(
                token.Header.Algorithm);

            var signatureBase = token.ToEncodedWebToken(false);

            var isSignatureValid = verifier.VerifyText(
                _key,
                Convert.FromBase64String(token.Signature),
                Encoding.UTF8.GetBytes(signatureBase));

            if (!isSignatureValid)
            {
                return new TokenValidationResult(
                    false, 
                    StringResources.InvalidJsonWebTokenSignature);
            }
            else
            {
                return new TokenValidationResult(true);
            }
        }
    }
}
