using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Quantfabric.Test.OAuthServer
{
    [DataContract]
    public class OAuth2AuthorizationRequest
    {
        public ResponseTypeEnum ResponseType => _responseType.StringToEnum<ResponseTypeEnum>();

        [DataMember(Name = "response_type")]
        private string _responseType;

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
