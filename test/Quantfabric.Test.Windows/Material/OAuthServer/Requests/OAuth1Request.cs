using System.Runtime.Serialization;

namespace Quantfabric.Test.Material.OAuthServer.Requests
{
    [DataContract]
    public class OAuth1Request
    {
        [DataMember(Name = "oauth_consumer_key")]
        public string ConsumerKey { get; set; }

        [DataMember(Name = "oauth_verifier")]
        public string Verifier { get; set; }

        [DataMember(Name = "oauth_callback")]
        public string RedirectUri { get; set; }

        [DataMember(Name = "oauth_token")]
        public string OAuthToken { get; set; }

        [DataMember(Name = "oauth_version")]
        public string OAuthVersion { get; set; }

        [DataMember(Name = "oauth_signature_method")]
        public string SignatureMethod { get; set; }

        [DataMember(Name = "oauth_timestamp")]
        public string Timestamp { get; set; }

        [DataMember(Name = "oauth_nonce")]
        public string Nonce { get; set; }

        [DataMember(Name = "oauth_signature")]
        public string Signature { get; set; }
    }
}
