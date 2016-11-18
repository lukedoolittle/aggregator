using System;
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

            var token = new JsonWebToken(
                deserializer.Deserialize<JsonWebTokenHeader>(header),
                deserializer.Deserialize<JsonWebTokenClaims>(claims),
                instance);

            return token;
        }
    }
}
