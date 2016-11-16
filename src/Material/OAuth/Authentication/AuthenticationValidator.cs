using System;
using System.Collections.Generic;
using System.Security;
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
        private readonly CryptoKey _key;
        private readonly IJsonWebTokenSigningFactory _factory;
        private readonly JsonWebTokenSigningTemplate _signingTemplate;
        private readonly List<JsonWebTokenAlgorithm> _whitelistedAlgorithms;

        public AuthenticationValidator(CryptoKey key) : 
                this(
                    key, 
                    new JsonWebTokenSignerFactory(), 
                    AuthenticationConfiguration.WhitelistedAlgorithms)
        { }

        public AuthenticationValidator(
            CryptoKey key, 
            IJsonWebTokenSigningFactory factory,
            List<JsonWebTokenAlgorithm> whitelistedAlgorithms)
        {
            _key = key;
            _factory = factory;
            _whitelistedAlgorithms = whitelistedAlgorithms;

            _signingTemplate = new JsonWebTokenSigningTemplate(factory);
        }

        public virtual bool IsTokenValid(JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (!_whitelistedAlgorithms.Contains(token.Header.Algorithm))
            {
                throw new SecurityException(
                    StringResources.InvalidJsonWebTokenAlgorithm);
            }
            if (DateTime.Now > token.Claims.ExpirationTime)
            {
                return false;
            }

            var verifier = _factory.GetVerificationAlgorithm(
                token.Header.Algorithm);

            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signatureBase = _signingTemplate.CreateSignatureBase(
                header,
                claims);

            return verifier.VerifyText(
                _key,
                Convert.FromBase64String(token.Signature),
                Encoding.UTF8.GetBytes(signatureBase));
        }
    }
}
