using System;
using System.Text;
using Foundations.Extensions;
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

            var header = splitEntity[0].FromBase64String();
            var claims = splitEntity[1].FromBase64String();

            return new JsonWebToken
            {
                Header = deserializer.Deserialize<JsonWebTokenHeader>(header),
                Claims = deserializer.Deserialize<JsonWebTokenClaims>(claims),
                Signature = splitEntity[2]
            };
        }

        public static string ToEncodedWebToken(
            this JsonWebToken instance)
        {
            return ToEncodedWebToken(instance, true);
        }

        public static string ToEncodedWebToken(
            this JsonWebToken instance,
            bool includeSignature)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return ToEncodedWebToken(
                instance.Header,
                instance.Claims, 
                includeSignature ? instance.Signature : string.Empty);
        }

        private static string ToEncodedWebToken(
            JsonWebTokenHeader header, 
            JsonWebTokenClaims claims, 
            string signature)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            if (claims == null) throw new ArgumentNullException(nameof(claims));

            var serializer = new JsonSerializer();
            var serializedHeader = serializer.Serialize(header);
            var serializedClaims = serializer.Serialize(claims);

            var headerBytes = Encoding.UTF8.GetBytes(serializedHeader);
            var headerEncoded = Convert.ToBase64String(headerBytes);

            var claimsBytes = Encoding.UTF8.GetBytes(serializedClaims.Replace("\\", ""));
            var claimsEncoded = Convert.ToBase64String(claimsBytes);

            var result = StringExtensions.Concatenate(
                headerEncoded,
                claimsEncoded,
                ".");

            if (!string.IsNullOrEmpty(signature))
            {
                result = StringExtensions.Concatenate(
                    result,
                    signature,
                    ".");
            }

            return result;
        }
    }
}
