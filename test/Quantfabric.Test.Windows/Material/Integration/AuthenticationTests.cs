using System;
using Material.Domain.Credentials;
using Material;
using Material.Application;
using Material.Authentication.Identities;
using Material.Authentication.Validation;
using Material.Domain.ResourceProviders;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Enums;
using Material.HttpClient.Cryptography.Keys;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    [Trait("Category", "RequiresToken")]
    public class AuthenticationTests
    {
        private readonly TokenCredentialRepository _tokenRepository
            = new TokenCredentialRepository(true);
        private readonly Randomizer _randomizer = 
            new Randomizer();

        public AuthenticationTests()
        {
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.ES256);
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.ES384);
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.ES512);

            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.RS256);
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.RS384);
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.RS512);

            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.HS256);
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.HS384);
            QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Add(JsonWebTokenAlgorithm.HS512);
        }

        [Fact]
        public async void CreateJsonWebTokenFromFacebookAccessToken()
        {
            var parameters = GenerateRandomParameters();
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();
            var recipient = Guid.NewGuid().ToString();
            var applicationName = Guid.NewGuid().ToString();

            var authenticator = new AuthenticationGenerator(
                parameters.PrivateKey, 
                recipient, 
                applicationName);

            var token = await authenticator
                .ConvertToJsonWebToken<FacebookIdentity>(
                    credentials, 
                    parameters.Algorithm)
                .ConfigureAwait(false);

            var validator = new JsonWebTokenSignatureValidator(
                parameters.PublicKey, 
                new JsonWebTokenSignerFactory());

            var tokenValidity = validator.IsTokenValid(token);

            Assert.True(tokenValidity.IsTokenValid);
        }

        [Fact]
        public async void CreateJsonWebTokenFromGoogleAccessToken()
        {
            var parameters = GenerateRandomParameters();
            var credentials = _tokenRepository.GetToken<Google, OAuth2Credentials>();
            var recipient = Guid.NewGuid().ToString();
            var applicationName = Guid.NewGuid().ToString();

            var authenticator = new AuthenticationGenerator(
                parameters.PrivateKey,
                recipient,
                applicationName);

            var token = await authenticator
                .ConvertToJsonWebToken<GoogleIdentity>(
                    credentials,
                    parameters.Algorithm)
                .ConfigureAwait(false);

            var validator = new JsonWebTokenSignatureValidator(
                parameters.PublicKey,
                new JsonWebTokenSignerFactory());

            var tokenValidity = validator.IsTokenValid(token);

            Assert.True(tokenValidity.IsTokenValid);
        }

        [Fact]
        public async void CreateJsonWebTokenFromFitbitAccessToken()
        {
            var parameters = GenerateRandomParameters();
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();
            var recipient = Guid.NewGuid().ToString();
            var applicationName = Guid.NewGuid().ToString();

            var authenticator = new AuthenticationGenerator(
                parameters.PrivateKey,
                recipient,
                applicationName);

            var token = await authenticator
                .ConvertToJsonWebToken<FitbitIdentity>(
                    credentials,
                    parameters.Algorithm)
                .ConfigureAwait(false);

            var validator = new JsonWebTokenSignatureValidator(
                parameters.PublicKey,
                new JsonWebTokenSignerFactory());

            var tokenValidity = validator.IsTokenValid(token);

            Assert.True(tokenValidity.IsTokenValid);
        }

        [Fact]
        public async void CreateJsonWebTokenFromTwitterAccessToken()
        {
            var parameters = GenerateRandomParameters();
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();
            var recipient = Guid.NewGuid().ToString();
            var applicationName = Guid.NewGuid().ToString();

            var authenticator = new AuthenticationGenerator(
                parameters.PrivateKey,
                recipient,
                applicationName);

            var token = await authenticator
                .ConvertToJsonWebToken<TwitterIdentity>(
                    credentials,
                    parameters.Algorithm)
                .ConfigureAwait(false);

            var validator = new JsonWebTokenSignatureValidator(
                parameters.PublicKey,
                new JsonWebTokenSignerFactory());

            var tokenValidity = validator.IsTokenValid(token);

            Assert.True(tokenValidity.IsTokenValid);
            Assert.Equal(credentials.ExternalUserId, token.Claims.Subject);
        }

        private JsonWebTokenParameters GenerateRandomParameters(
            JsonWebTokenAlgorithm algorithm)
        {
            if (algorithm == JsonWebTokenAlgorithm.HS256 ||
                algorithm == JsonWebTokenAlgorithm.HS384 ||
                algorithm == JsonWebTokenAlgorithm.HS512)
            {
                var key = _randomizer.RandomString(100);
                return new JsonWebTokenParameters(
                    new HashKey(key, StringEncoding.Utf8),
                    new HashKey(key, StringEncoding.Utf8),
                    algorithm);
            }
            else if (algorithm == JsonWebTokenAlgorithm.ES256 ||
                     algorithm == JsonWebTokenAlgorithm.ES384 ||
                     algorithm == JsonWebTokenAlgorithm.ES512)
            {
                var curveName = "P-256";
                var keyPair = EcdsaCryptoKeyPair.Create(curveName);
                return new JsonWebTokenParameters(
                    keyPair.Private,
                    keyPair.Public,
                    algorithm);
            }
            else
            {
                var keyPair = RsaCryptoKeyPair.Create(1024);
                return new JsonWebTokenParameters(
                    keyPair.Private,
                    keyPair.Public,
                    algorithm);
            }
        }

        private JsonWebTokenParameters GenerateRandomParameters()
        {
            var algorithmIndex = _randomizer.RandomNumber(
                0,
                QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms.Count - 1);
            var algorithm = QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms[algorithmIndex];

            return GenerateRandomParameters(algorithm);
        }
    }

    public class JsonWebTokenParameters
    {
        public CryptoKey PrivateKey { get; }
        public CryptoKey PublicKey { get; }
        public JsonWebTokenAlgorithm Algorithm { get; }

        public JsonWebTokenParameters(
            CryptoKey privateKey,
            CryptoKey publicKey,
            JsonWebTokenAlgorithm algorithm)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
            Algorithm = algorithm;
        }
    }
}
