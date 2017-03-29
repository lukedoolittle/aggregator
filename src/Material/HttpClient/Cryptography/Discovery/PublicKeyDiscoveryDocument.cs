using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.HttpClient.Cryptography.Discovery
{
    [GeneratedCode("JsonUtils", "1.0")]
    [DataContract]
    public class JsonWebKey
    {
        [DataMember(Name = "kty")]
        public string KeyType { get; set; }

        [DataMember(Name = "alg")]
        public string Algorithm { get; set; }

        [DataMember(Name = "use")]
        public string Use { get; set; }

        [DataMember(Name = "kid")]
        public string KeyId { get; set; }

        [DataMember(Name = "n")]
        public string N { get; set; }

        [DataMember(Name = "e")]
        public string E { get; set; }

        [DataMember(Name = "crv")]
        public string CurveName { get; set; }

        [DataMember(Name = "x")]
        public string X { get; set; }

        [DataMember(Name = "y")]
        public string Y { get; set; }

        [DataMember(Name = "x5t")]
        public string X5T { get; set; }

        [DataMember(Name = "x5c")]
        public IList<string> X5C { get; set; }
    }

    [GeneratedCode("JsonUtils", "1.0")]
    [DataContract]
    public class PublicKeyDiscoveryDocument
    {
        [DataMember(Name = "keys")]
        public IList<JsonWebKey> Keys { get; set; }
    }
}
