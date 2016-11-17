using System;
using System.Collections.Generic;
using System.Text;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class AuthenticationValidator
    {
        private readonly IJsonWebTokenSigningFactory _factory;
        private readonly JsonWebTokenSigningTemplate _signingTemplate;
        private readonly List<JsonWebTokenAlgorithm> _whitelistedAlgorithms;

        public AuthenticationValidator() : 
                this(new JsonWebTokenSignerFactory())
        { }

        public AuthenticationValidator(
            IJsonWebTokenSigningFactory factory)
        {
            _factory = factory;
            _whitelistedAlgorithms = AuthenticationConfiguration.WhitelistedAlgorithms;

            _signingTemplate = new JsonWebTokenSigningTemplate(factory);
        }

        public virtual TokenValidationResult IsTokenValid(
            JsonWebToken token, 
            CryptoKey key)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (!_whitelistedAlgorithms.Contains(token.Header.Algorithm))
            {
                return new TokenValidationResult(
                    false, 
                    StringResources.InvalidJsonWebTokenAlgorithm);
            }
            if (DateTime.Now > token.Claims.ExpirationTime)
            {
                return new TokenValidationResult(
                    false, 
                    StringResources.WebTokenExpired);
            }

            var verifier = _factory.GetVerificationAlgorithm(
                token.Header.Algorithm);

            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signatureBase = _signingTemplate.CreateSignatureBase(
                header,
                claims);

            var isSignatureValid = verifier.VerifyText(
                key,
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
