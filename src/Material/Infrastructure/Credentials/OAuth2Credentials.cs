using System.Runtime.Serialization;
using Foundations.HttpClient.Enums;

namespace Material.Infrastructure.Credentials
{
    [DataContract]
    public class OAuth2Credentials : TokenCredentials
    {
        [DataMember(Name = "clientId")]
        public string ClientId { get; private set; }

        [DataMember(Name = "clientSecret")]
        public string ClientSecret { get; private set; }

        public string CallbackUrl { get; private set; }

        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        protected string _accessToken;
        [DataMember(Name = "accessToken", EmitDefaultValue = false)]
        protected string _accessTokenAlternate;
        public string AccessToken => _accessToken ?? _accessTokenAlternate;

        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        protected string _expiresIn;
        [DataMember(Name = "expires", EmitDefaultValue = false)]
        protected string _expiresInAlternate;
        public override string ExpiresIn => _expiresIn ?? _expiresInAlternate;

        public override bool AreValidIntermediateCredentials =>
            !string.IsNullOrEmpty(Code) ||
            !string.IsNullOrEmpty(AccessToken);

        [DataMember(Name = "refresh_token", EmitDefaultValue = false)]
        public string RefreshToken { get; private set; }

        [DataMember(Name = "token_type")]
        public string TokenName { get; private set; }

        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string Code { get; private set; }

        public string Scope { get; private set; }

        public override bool HasValidPublicKey => !string.IsNullOrEmpty(ClientId);

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
