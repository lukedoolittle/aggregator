using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.HttpClient.Cryptography.Discovery
{
    [GeneratedCode("JsonUtils", "1.0")]
    [DataContract]
    public class OpenIdConnectDiscoveryDocument
    {
        [DataMember(Name = "issuer")]
        public string Issuer { get; set; }

        [DataMember(Name = "authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [DataMember(Name = "token_endpoint")]
        public string TokenEndpoint { get; set; }

        [DataMember(Name = "userinfo_endpoint")]
        public string UserinfoEndpoint { get; set; }

        [DataMember(Name = "revocation_endpoint")]
        public string RevocationEndpoint { get; set; }

        [DataMember(Name = "jwks_uri")]
        public string JsonWebKeysUri { get; set; }

        [DataMember(Name = "response_types_supported")]
        public IList<string> ResponseTypesSupported { get; set; }

        [DataMember(Name = "subject_types_supported")]
        public IList<string> SubjectTypesSupported { get; set; }

        [DataMember(Name = "id_token_signing_alg_values_supported")]
        public IList<string> IdTokenSigningAlgValuesSupported { get; set; }

        [DataMember(Name = "scopes_supported")]
        public IList<string> ScopesSupported { get; set; }

        [DataMember(Name = "token_endpoint_auth_methods_supported")]
        public IList<string> TokenEndpointAuthMethodsSupported { get; set; }

        [DataMember(Name = "claims_supported")]
        public IList<string> ClaimsSupported { get; set; }

        [DataMember(Name = "code_challenge_methods_supported")]
        public IList<string> CodeChallengeMethodsSupported { get; set; }
    }
}
