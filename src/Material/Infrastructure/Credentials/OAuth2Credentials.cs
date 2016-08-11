using System.Runtime.Serialization;
using Material.Enums;
using Newtonsoft.Json;

namespace Material.Infrastructure.Credentials
{
    [DataContract]
    public class OAuth2Credentials : TokenCredentials
    {
        [DataMember(Name = "clientId")]
        [JsonProperty("clientId")]
        public string ClientId { get; private set; }

        [DataMember(Name = "clientSecret")]
        [JsonProperty("clientSecret")]
        public string ClientSecret { get; private set; }

        public string CallbackUrl { get; private set; }

        [DataMember(Name = "access_token")]
        [JsonProperty("access_token")]
        protected string _accessToken;
        [DataMember(Name = "accessToken")]
        [JsonProperty("accessToken")]
        protected string _accessTokenAlternate;
        [JsonIgnore]
        public string AccessToken => _accessToken ?? _accessTokenAlternate;

        [DataMember(Name = "expires_in")]
        [JsonProperty("expires_in")]
        protected string _expiresIn;
        [DataMember(Name = "expires")]
        [JsonProperty("expires")]
        protected string _expiresInAlternate;
        [JsonIgnore]
        public override string ExpiresIn => _expiresIn ?? _expiresInAlternate;

        public override bool AreValidIntermediateCredentials =>
            !string.IsNullOrEmpty(Code) ||
            !string.IsNullOrEmpty(AccessToken);

        [DataMember(Name = "refresh_token")]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; private set; }

        [DataMember(Name = "token_type")]
        [JsonProperty("token_type")]
        public string TokenName { get; private set; }

        [DataMember(Name = "code")]
        [JsonProperty("code")]
        public string Code { get; private set; }

        public string Scope { get; private set; }

        [JsonIgnore]
        public override bool HasValidPublicKey => !string.IsNullOrEmpty(ClientId);

        [JsonIgnore]
        public ResponseTypeEnum ResponseType => string.IsNullOrEmpty(ClientSecret) ? 
                ResponseTypeEnum.Token : 
                ResponseTypeEnum.Code;

        public OAuth2Credentials SetTokenName(string tokenName)
	    {
	        TokenName = tokenName;
	        return this;
	    }

        public OAuth2Credentials SetClientProperties(
            string clientId,
            string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            return this;
        }

        public OAuth2Credentials TransferRefreshToken(
            string refreshToken)
	    {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                RefreshToken = refreshToken;
            }

            return this;
	    }

        public OAuth2Credentials SetCallbackUrl(string callbackUrl)
        {
            CallbackUrl = callbackUrl;

            return this;
        }
    }
}
