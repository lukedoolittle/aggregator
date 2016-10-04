using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Quantfabric.Test.OAuthServer
{
    [DataContract]
    public class OAuth2TokenRequest
    {
        public GrantTypeEnum GrantType => _grantType.StringToEnum<GrantTypeEnum>();

        [DataMember(Name = "grant_type")]
        private string _grantType;

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "client_secret")]
        public string ClientSecret { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "assertion")]
        public string JsonWebToken { get; set; }

        [DataMember(Name = "redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
