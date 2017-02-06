using System;
using Foundations.Extensions;
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

            var isSignatureValid = verifier.VerifyMessage(
                _key,
                token.Signature.FromBase64String(),
                token.SignatureBase);

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
