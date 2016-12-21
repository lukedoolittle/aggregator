using System;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Serialization;

namespace Material.Infrastructure.Credentials
{
    public static class JsonWebTokenExtensions
    {
        public static JsonWebToken ToWebToken(this string instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var splitEntity = instance.Split('.');

            var deserializer = new JsonSerializer();

            var header = splitEntity[0].Base64ToUtf8String();
            var claims = splitEntity[1].Base64ToUtf8String();

            var token = new JsonWebToken(
                deserializer.Deserialize<JsonWebTokenHeader>(header),
                deserializer.Deserialize<JsonWebTokenClaims>(claims),
                instance);

            return token;
        }

        public static JsonWebToken Sign(
            this JsonWebToken instance, 
            IJsonWebTokenSigningFactory signingFactory,
            CryptoKey privateKey)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (signingFactory == null) throw new ArgumentNullException(nameof(signingFactory));

            var signingAlgorithm = signingFactory
                .GetSigningAlgorithm(
                    instance.Header.Algorithm);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(
                instance.SignatureBase);

            var signatureBytes = signingAlgorithm.SignText(
                signatureBaseBytes,
                privateKey);

            var signature = Convert.ToBase64String(signatureBytes);

            instance.Sign(instance.SignatureBase, signature);

            return instance;
        }
    }
}
