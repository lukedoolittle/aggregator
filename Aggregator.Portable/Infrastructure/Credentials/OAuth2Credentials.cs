using System;
using Newtonsoft.Json;

namespace Aggregator.Infrastructure.Credentials
{
    public class OAuth2Credentials : TokenCredentials
    {
        [JsonProperty("clientId")]
        public string ClientId { get; private set; }

        [JsonProperty("clientSecret")]
        public string ClientSecret { get; private set; }

        [JsonProperty("access_token")]
        private string _accessToken;

        [JsonProperty("accessToken")]
        private string _accessTokenAlternate;

        [JsonIgnore]
        public string AccessToken => _accessToken ?? _accessTokenAlternate;

        [JsonProperty("expires_in")]
        private string _expiresIn;

        [JsonProperty("expires")]
        private string _expiresInAlternate;

        [JsonIgnore]
        public string ExpiresIn => _expiresIn ?? _expiresInAlternate;

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; private set; }

        [JsonProperty("tokenName")]
        public string TokenName { get; private set; }

        [JsonProperty("code")]
        public string Code { get; private set; }

        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; private set; }

        [JsonIgnore]
        public override bool IsTokenExpired => !IsTokenValid();

        [JsonIgnore]
        public override bool HasValidProperties =>
            !(string.IsNullOrEmpty(ClientId) ||
              string.IsNullOrEmpty(ClientSecret));

        private bool IsTokenValid()
        {
            if (string.IsNullOrEmpty(ExpiresIn))
            {
                return true;
            }
            else
            {
                var secondsUntilExpiration = Convert.ToInt32(ExpiresIn);
                var secondsSinceCreation = (DateTimeOffset.Now - DateCreated).TotalSeconds;
                return secondsUntilExpiration > secondsSinceCreation;
            }
        }

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

	    public void TimestampToken()
	    {
	        DateCreated = DateTimeOffset.Now;
	    }
    }
}
