using System;
using System.Runtime.Serialization;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient.Request
{
    [DataContract]
    public class JsonWebToken
    {
        public JsonWebTokenHeader Header { get; set; } = 
            new JsonWebTokenHeader();

        public JsonWebTokenClaims Claims { get; set; } = 
            new JsonWebTokenClaims();

        public string Signature { get; set; }

        public JsonWebToken() { }

        public JsonWebToken(
            JsonWebTokenHeader header, 
            JsonWebTokenClaims claims,
            string signature)
        {
            Header = header;
            Claims = claims;
            Signature = signature;
        }

        public static JsonWebToken FromString(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var splitEntity = token.Split('.');

            var deserializer = new JsonSerializer();

            var header = splitEntity[0].FromBase64String();
            var claims = splitEntity[1].FromBase64String();

            return new JsonWebToken(
                deserializer.Deserialize<JsonWebTokenHeader>(header),
                deserializer.Deserialize<JsonWebTokenClaims>(claims),
                splitEntity[2]);
        }

        public override string ToString()
        {
            var serializer = new JsonSerializer();

            var header = serializer.Serialize(Header);
            var headerBytes = Encoding.UTF8.GetBytes(header);
            var headerEncoded = Convert.ToBase64String(headerBytes);

            //TODO: is there a more elegant way to remove escapes??
            var claims = serializer.Serialize(Claims).Replace("\\", ""); ;
            var claimsBytes = Encoding.UTF8.GetBytes(claims);
            var claimsEncoded = Convert.ToBase64String(claimsBytes);

            var signatureBase = StringExtensions.Concatenate(
                headerEncoded,
                claimsEncoded,
                ".");

            if (!string.IsNullOrEmpty(Signature))
            {
                return StringExtensions.Concatenate(
                    signatureBase,
                    Signature,
                    ".");
            }
            else
            {
                return signatureBase;
            }
        }
    }

    [DataContract]
    public class JsonWebTokenHeader
    {
        [DataMember(Name = "typ", Order = 0)]
        public string MediaType { get; set; } = "JWT";

        public JsonWebTokenAlgorithm Algorithm { get; set; } = JsonWebTokenAlgorithm.RS256;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "alg", Order = 1)]
        private string _algorithm
        {
            get { return Algorithm.EnumToString(); }
            set { Algorithm = value.StringToEnum<JsonWebTokenAlgorithm>(); }
        }
    }

    [DataContract]
    public class JsonWebTokenClaims
    {
        [DataMember(Name = "iss", Order = 0, EmitDefaultValue = false)]
        public string Issuer { get; set; }

        [DataMember(Name = "scope", Order = 1, EmitDefaultValue = false)]
        public string Scope { get; set; }

        [DataMember(Name = "aud", Order = 2, EmitDefaultValue = false)]
        public string Audience { get; set; }

        [DataMember(Name = "exp", Order = 4, EmitDefaultValue = false)]
        public double ExpirationTime { get; set; }

        [DataMember(Name = "iat", Order = 3, EmitDefaultValue = false)]
        public double IssuedAt { get; set; }

        [DataMember(Name = "sub", Order = 5, EmitDefaultValue = false)]
        public string Subject { get; set; }

        [DataMember(Name = "nbf", Order = 6, EmitDefaultValue = false)]
        public string NotBefore { get; set; }

        [DataMember(Name = "jti", Order = 7, EmitDefaultValue = false)]
        public string Id { get; set; }
    }
}
