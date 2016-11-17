using System;
using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Enums;

namespace Material.Infrastructure.Credentials
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
    }

    [DataContract]
    public class JsonWebTokenHeader
    {
        //Type of the token provided; the only valid value is JWT
        [DataMember(Name = "typ", Order = 0, EmitDefaultValue = false)]
        public string MediaType { get; set; } = "JWT";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "alg", Order = 1, EmitDefaultValue = false)]
        private string _algorithm
        {
            get { return Algorithm.EnumToString(); }
            set { Algorithm = value.StringToEnum<JsonWebTokenAlgorithm>(); }
        }
        //Algorithm used to sign the token
        public JsonWebTokenAlgorithm Algorithm { get; set; } = JsonWebTokenAlgorithm.RS256;

        //The identifier of the key used to sign the token
        [DataMember(Name = "kid", Order = 2, EmitDefaultValue = false)]
        public string SignatureKeyId { get; set; }
    }

    [DataContract]
    public class JsonWebTokenClaims
    {
        //The Issuer Identifier for the Issuer of the response
        [DataMember(Name = "iss", Order = 0, EmitDefaultValue = false)]
        public string Issuer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "iat", EmitDefaultValue = false, Order = 1)]
        private double _issuedAt
        {
            get { return Math.Floor(IssuedAt.ToUnixTimeSeconds()); }
            set { IssuedAt = value.FromUnixTimeSeconds().DateTime; }
        }
        //The time the ID token was issued, represented in Unix time (integer seconds)
        public DateTime IssuedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "exp", EmitDefaultValue = false, Order = 2)]
        private double _expirationTime
        {
            get { return Math.Floor(ExpirationTime.ToUnixTimeSeconds()); }
            set { ExpirationTime = value.FromUnixTimeSeconds().DateTime; }
        }
        //The time the ID token expires, represented in Unix time (integer seconds)
        public DateTime ExpirationTime { get; set; }

        [DataMember(Name = "at_hash", EmitDefaultValue = false, Order = 3)]
        public string AccessTokenHash { get; set; }

        //Identifies the audience that this ID token is intended for. Typically one of the client IDs of your application
        [DataMember(Name = "aud", EmitDefaultValue = false, Order = 4)]
        public string Audience { get; set; }

        //An identifier for the user, unique among all accounts and never reused
        [DataMember(Name = "sub", EmitDefaultValue = false, Order = 5)]
        public string Subject { get; set; }

        //The client_id of the authorized presenter
        [DataMember(Name = "azp", EmitDefaultValue = false, Order = 6)]
        public string AuthorizedPresenter { get; set; }

        [DataMember(Name = "scope", EmitDefaultValue = false, Order = 7)]
        public string Scope { get; set; }

        //[DataMember(Name = "nbf", EmitDefaultValue = false)]
        //public string NotBefore { get; set; }

        //[DataMember(Name = "jti", EmitDefaultValue = false)]
        //public string Id { get; set; }
    }
}
