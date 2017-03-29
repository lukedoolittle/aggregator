using System.Runtime.Serialization;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace Quantfabric.Test.Material.OAuthServer.Requests
{
    [DataContract]
    public class OAuth2TokenRequest
    {
        public GrantType GrantType => _grantType.StringToEnum<GrantType>();

        [DataMember(Name = "grant_type")]
#pragma warning disable 0649
        private string _grantType;
#pragma warning restore 0649

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
