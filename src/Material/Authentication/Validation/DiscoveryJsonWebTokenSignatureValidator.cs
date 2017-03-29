using System;
using Material.Domain.Credentials;
using System.Linq;
using Material.Contracts;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Discovery;
using Material.HttpClient.Extensions;

namespace Material.Authentication.Validation
{
    public class DiscoveryJsonWebTokenSignatureValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly Uri _discoveryUrl;

        public DiscoveryJsonWebTokenSignatureValidator(Uri discoveryUrl)
        {
            _discoveryUrl = discoveryUrl;
        }

        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            var discoveryDocument = new HttpRequestBuilder(_discoveryUrl.NonPath())
                .GetFrom(_discoveryUrl.AbsolutePath)
                .ResultAsync<OpenIdConnectDiscoveryDocument>()
                .Result;

            var keysUrl = new Uri(discoveryDocument.JsonWebKeysUri);

            var keys = new HttpRequestBuilder(keysUrl.NonPath())
                .GetFrom(keysUrl.AbsolutePath)
                .ResultAsync<PublicKeyDiscoveryDocument>()
                .Result;

            var key = keys
                .Keys
                .SingleOrDefault(k => k.KeyId == token.Header.SignatureKeyId);

            if (key == null)
            {
                return new TokenValidationResult(
                    false, 
                    StringResources.MissingPublicKey);
            }

            return new JsonWebTokenSignatureValidator(
                    key.ToCryptoKey(), 
                    new JsonWebTokenSignerFactory())
                .IsTokenValid(token);
        }
    }
}
