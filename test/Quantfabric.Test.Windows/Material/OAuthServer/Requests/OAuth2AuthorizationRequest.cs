using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Quantfabric.Test.OAuthServer
{
    [DataContract]
    public class OAuth2AuthorizationRequest
    {
        public OAuth2ResponseType ResponseType => _responseType.StringToEnum<OAuth2ResponseType>();

        [DataMember(Name = "response_type")]
#pragma warning disable 0649
        private string _responseType;
#pragma warning restore 0649

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "redirect_uri")]
        public string RedirectUri { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }
}
