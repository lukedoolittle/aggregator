using System;
using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Serialization;

namespace Material.Infrastructure.Credentials
{
    //TODO: should override GetHashCode() for this value object
    [DataContract]
    public class JsonWebToken
    {
        public JsonWebTokenHeader Header { get; set; } = 
            new JsonWebTokenHeader();

        public JsonWebTokenClaims Claims { get; set; } = 
            new JsonWebTokenClaims();

        public string Signature { get; set; }

        public JsonWebToken() { }

        public JsonWebToken(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var splitEntity = token.Split('.');

            var deserializer = new JsonSerializer();

            var header = splitEntity[0].FromBase64String();
            var claims = splitEntity[1].FromBase64String();

            Header = deserializer.Deserialize<JsonWebTokenHeader>(header);
            Claims = deserializer.Deserialize<JsonWebTokenClaims>(claims);
            Signature = splitEntity[2];
        }

        public JsonWebToken(
            JsonWebTokenHeader header, 
            JsonWebTokenClaims claims,
            string signature)
        {
            Header = header;
            Claims = claims;
            Signature = signature;
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

        public DateTime ExpirationTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "exp", Order = 4, EmitDefaultValue = false)]
        private double _expirationTime
        {
            get { return Math.Floor(ExpirationTime.ToUnixTimeSeconds()); }
            set { ExpirationTime = value.FromUnixTimeSeconds().DateTime; }
        }

        public DateTime IssuedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "iat", Order = 3, EmitDefaultValue = false)]
        private double _issuedAt
        {
            get { return Math.Floor(IssuedAt.ToUnixTimeSeconds()); }
            set { IssuedAt = value.FromUnixTimeSeconds().DateTime; }
        }

        [DataMember(Name = "sub", Order = 5, EmitDefaultValue = false)]
        public string Subject { get; set; }

        [DataMember(Name = "nbf", Order = 6, EmitDefaultValue = false)]
        public string NotBefore { get; set; }

        [DataMember(Name = "jti", Order = 7, EmitDefaultValue = false)]
        public string Id { get; set; }
    }
}
