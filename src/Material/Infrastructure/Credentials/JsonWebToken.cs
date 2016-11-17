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
        [DataMember(Name = "typ")]
        public string MediaType { get; set; } = "JWT";

        //Algorithm used to sign the token
        public JsonWebTokenAlgorithm Algorithm { get; set; } = JsonWebTokenAlgorithm.RS256;

        //The identifier of the key used to sign the token
        [DataMember(Name = "kid")]
        public string SignatureKeyId { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "alg")]
        private string _algorithm
        {
            get { return Algorithm.EnumToString(); }
            set { Algorithm = value.StringToEnum<JsonWebTokenAlgorithm>(); }
        }
    }

    [DataContract]
    public class JsonWebTokenClaims
    {
        //The Issuer Identifier for the Issuer of the response
        [DataMember(Name = "iss", EmitDefaultValue = false)]
        public string Issuer { get; set; }

        //An identifier for the user, unique among all accounts and never reused
        [DataMember(Name = "sub", EmitDefaultValue = false)]
        public string Subject { get; set; }

        //Identifies the audience that this ID token is intended for. Typically one of the client IDs of your application
        [DataMember(Name = "aud", EmitDefaultValue = false)]
        public string Audience { get; set; }

        //The time the ID token expires, represented in Unix time (integer seconds)
        public DateTime ExpirationTime { get; set; }

        //The time the ID token was issued, represented in Unix time (integer seconds)
        public DateTime IssuedAt { get; set; }

        [DataMember(Name = "nbf", EmitDefaultValue = false)]
        public string NotBefore { get; set; }

        [DataMember(Name = "jti", EmitDefaultValue = false)]
        public string Id { get; set; }

        //The client_id of the authorized presenter
        [DataMember(Name = "azp", EmitDefaultValue = false)]
        public string AuthorizedPresenter { get; set; }

        [DataMember(Name = "at_hash", EmitDefaultValue = false)]
        public string AccessTokenHash { get; set; }

        [DataMember(Name = "scope", EmitDefaultValue = false)]
        public string Scope { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "iat", EmitDefaultValue = false)]
        private double _issuedAt
        {
            get { return Math.Floor(IssuedAt.ToUnixTimeSeconds()); }
            set { IssuedAt = value.FromUnixTimeSeconds().DateTime; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "exp", EmitDefaultValue = false)]
        private double _expirationTime
        {
            get { return Math.Floor(ExpirationTime.ToUnixTimeSeconds()); }
            set { ExpirationTime = value.FromUnixTimeSeconds().DateTime; }
        }
    }
}
