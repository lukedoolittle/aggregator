using System;
using System.Globalization;
using System.Runtime.Serialization;
using Foundations.Extensions;

namespace Material.Infrastructure.Credentials
{
    //TODO: should override GetHashCode() for this value object
    [DataContract]
    public class OAuth2Credentials : TokenCredentials
    {
        [DataMember(Name = "clientId", EmitDefaultValue = false)]
        public string ClientId { get; private set; }

        [DataMember(Name = "clientSecret", EmitDefaultValue = false)]
        public string ClientSecret { get; private set; }

        [DataMember(Name = "privateKey", EmitDefaultValue = false)]
        public string PrivateKey { get; private set; }

        [DataMember(Name = "clientEmail", EmitDefaultValue = false)]
        public string ClientEmail { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string CallbackUrl { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        protected string _accessToken;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "accessToken", EmitDefaultValue = false)]
        protected string _accessTokenAlternate;
        public string AccessToken => _accessToken ?? _accessTokenAlternate;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        protected string _expiresIn;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "expires", EmitDefaultValue = false)]
        protected string _expiresInAlternate;
        public override string ExpiresIn => _expiresIn ?? _expiresInAlternate;

        public override bool AreValidIntermediateCredentials =>
            !string.IsNullOrEmpty(Code) ||
            !string.IsNullOrEmpty(AccessToken);

        [DataMember(Name = "refresh_token", EmitDefaultValue = false)]
        public string RefreshToken { get; private set; }

        [DataMember(Name = "token_type", EmitDefaultValue = false)]
        public string TokenName { get; private set; }

        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string Code { get; private set; }

        public string Scope { get; private set; }

        public override bool HasValidPublicKey => !string.IsNullOrEmpty(ClientId);

        public OAuth2Credentials SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            return this;
        }

        public OAuth2Credentials SetExpiresIn(double expiryTime)
        {
            var expiryDateTime = expiryTime.FromUnixTimeSeconds();
            _expiresIn = ((int)expiryDateTime
                .Subtract(DateTime.UtcNow)
                .TotalSeconds)
                .ToString(CultureInfo.InvariantCulture);

            return this;
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

        public OAuth2Credentials SetClientProperties(
            string clientId, 
            string privateKey, 
            string email)
        {
            ClientId = clientId;
            PrivateKey = privateKey;
            ClientEmail = email;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public OAuth2Credentials SetCallbackUrl(string callbackUrl)
        {
            CallbackUrl = callbackUrl;

            return this;
        }

        public OAuth2Credentials TimestampToken()
        {
            DateCreated = DateTimeOffset.Now;

            return this;
        }
    }
}
