using System;
using System.Runtime.Serialization;
using Foundations.Cryptography.JsonWebToken;

namespace Foundations.HttpClient.Request
{
    [DataContract]
    public class JsonWebToken
    {
        public JsonWebTokenHeader Header { get; set; } = 
            new JsonWebTokenHeader();

        public JsonWebTokenClaims Claims { get; set; } = 
            new JsonWebTokenClaims();
    }

    [DataContract]
    public class JsonWebTokenHeader
    {
        [DataMember(Name = "typ", Order = 0)]
        public string Type { get; set; } = "JWT";

        public JwtAlgorithm Algorithm { get; set; } = JwtAlgorithm.RS256;

        [DataMember(Name = "alg", Order = 1)]
        private string _algorithm
        {
            get { return Algorithm.ToString(); }
            set { throw new NotImplementedException(); }
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
        public string JWTId { get; set; }
    }
}
