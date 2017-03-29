using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Material.Contracts;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Enums;
using Material.HttpClient.Cryptography.Keys;

namespace Material.Application
{
    public class AuthenticationGenerator
    {
        private readonly CryptoKey _privateKey;
        private readonly string _applicationName;
        private readonly string _intendedRecipient;
        private readonly TimeSpan _expirationTime;
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
                    new JsonWebTokenSignerFactory(), 
                    QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms,
                    QuantfabricConfiguration.AuthenticationTokenTimeout)
        { }

        public AuthenticationGenerator(
            CryptoKey privateKey, 
            string applicationName,
            string recipient,
            IJsonWebTokenSigningFactory signingFactory,
            List<JsonWebTokenAlgorithm> whitelistedAlgorithms,
            TimeSpan authenticationTokenTimeout)
        {
            _privateKey = privateKey;
            _whitelistedAlgorithms = whitelistedAlgorithms;
            _expirationTime = authenticationTokenTimeout;
            _applicationName = applicationName;
            _intendedRecipient = recipient;
            _signingFactory = signingFactory;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public async Task<JsonWebToken> ConvertToJsonWebToken<TIdentity>(
            OAuth1Credentials credentials,
            JsonWebTokenAlgorithm algorithm)
            where TIdentity : IOAuthIdentity<OAuth1Credentials>, new()
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
            where TIdentity : IOAuthIdentity<OAuth2Credentials>, new()
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
                    ExpirationTime = DateTime.Now.Add(_expirationTime),
                    Issuer = _applicationName,
                    Audience = _intendedRecipient,
                });
        }
    }
}
