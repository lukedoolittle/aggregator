using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
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
        private readonly IJsonWebTokenSigningFactory _signingFactory;
        private readonly List<JsonWebTokenAlgorithm> _whitelistedAlgorithms;

        public AuthenticationGenerator(
            CryptoKey privateKey, 
            string recipient,
            string applicationName) : 
                this(
                    privateKey,
                    applicationName,
                    recipient,
                    new JsonWebTokenSignerFactory())
        { }

        public AuthenticationGenerator(
            CryptoKey privateKey, 
            string applicationName,
            string recipient,
            IJsonWebTokenSigningFactory signingFactory)
        {
            _privateKey = privateKey;
            _whitelistedAlgorithms = AuthenticationConfiguration.WhitelistedAlgorithms;
            _expirationTimeInMinutes = AuthenticationConfiguration.AuthenticationTokenTimeoutInMinutes;
            _applicationName = applicationName;
            _intendedRecipient = recipient;
            _signingFactory = signingFactory;
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
                    InitializeToken(algorithm),
                    credentials)
                .ConfigureAwait(false);

            return token.Sign(_signingFactory, _privateKey);
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
                    InitializeToken(algorithm),
                    credentials)
                .ConfigureAwait(false);

            return token.Sign(_signingFactory, _privateKey);
        }

        protected virtual JsonWebToken InitializeToken(
            JsonWebTokenAlgorithm algorithm)
        {
            if (!_whitelistedAlgorithms.Contains(algorithm))
            {
                throw new SecurityException(
                    StringResources.InvalidJsonWebTokenAlgorithm);
            }

            return new JsonWebToken(
                new JsonWebTokenHeader
                {
                    Algorithm = algorithm
                },
                new JsonWebTokenClaims
                {
                    IssuedAt = DateTime.Now,
                    ExpirationTime = DateTime.Now.Add(TimeSpan.FromMinutes(_expirationTimeInMinutes)),
                    Issuer = _applicationName,
                    Audience = _intendedRecipient,
                });
        }
    }
}
