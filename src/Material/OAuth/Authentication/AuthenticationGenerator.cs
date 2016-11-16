using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class AuthenticationGenerator
    {
        private readonly CryptoKey _privateKey;
        private readonly string _applicationName;
        private readonly string _intendedRecipient;
        private readonly int _expirationTimeInMinutes;
        private readonly JsonWebTokenSigningTemplate _signingTemplate;
        private readonly List<JsonWebTokenAlgorithm> _whitelistedAlgorithms;

        public AuthenticationGenerator(
            CryptoKey privateKey, 
            string recipient,
            string applicationName) : 
                this(
                    privateKey,
                    new JsonWebTokenSigningTemplate(
                        new JsonWebTokenSignerFactory()),
                    AuthenticationConfiguration.WhitelistedAlgorithms,
                    applicationName,
                    recipient,
                    AuthenticationConfiguration.AuthenticationTokenTimeoutInMinutes)
        { }

        public AuthenticationGenerator(
            CryptoKey privateKey, 
            JsonWebTokenSigningTemplate signingTemplate,
            List<JsonWebTokenAlgorithm> whitelistedAlgorithms,
            string applicationName,
            string recipient,
            int expirationTimeInMinutes)
        {
            _privateKey = privateKey;
            _whitelistedAlgorithms = whitelistedAlgorithms;
            _expirationTimeInMinutes = expirationTimeInMinutes;
            _applicationName = applicationName;
            _intendedRecipient = recipient;
            _signingTemplate = signingTemplate;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public async Task<JsonWebToken> ConvertToJsonWebToken<TIdentity>(
            OAuth1Credentials credentials,
            JsonWebTokenAlgorithm algorithm)
            where TIdentity : IOAuth1Identity, new()
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var token = await new TIdentity()
                .AppendIdentity(
                    InitializeToken(new JsonWebToken(), algorithm),
                    credentials)
                .ConfigureAwait(false);

            return SignToken(token);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public async Task<JsonWebToken> ConvertToJsonWebToken<TIdentity>(
            OAuth2Credentials credentials,
            JsonWebTokenAlgorithm algorithm)
            where TIdentity : IOAuth2Identity, new()
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var token = await new TIdentity()
                .AppendIdentity(
                    InitializeToken(new JsonWebToken(), algorithm),
                    credentials)
                .ConfigureAwait(false);

            return SignToken(token);
        }

        private JsonWebToken InitializeToken(
            JsonWebToken token,
            JsonWebTokenAlgorithm algorithm)
        {
            if (!_whitelistedAlgorithms.Contains(algorithm))
            {
                throw new SecurityException(
                    StringResources.InvalidJsonWebTokenAlgorithm);
            }

            token.Header = new JsonWebTokenHeader
            {
                Algorithm = algorithm
            };
            token.Claims = new JsonWebTokenClaims
            {
                IssuedAt = DateTime.Now,
                ExpirationTime = DateTime.Now.Add(TimeSpan.FromMinutes(_expirationTimeInMinutes)),
                Issuer = _applicationName,
                Audience = _intendedRecipient
            };

            return token;
        }

        private JsonWebToken SignToken(JsonWebToken token)
        {
            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signatureBase = _signingTemplate.CreateSignatureBase(
                header,
                claims);

            token.Signature = _signingTemplate.CreateSignature(
                signatureBase,
                token.Header.Algorithm,
                _privateKey);

            return token;
        }
    }
}
