using System.Runtime.Serialization;
using Foundations.HttpClient.Enums;
using Newtonsoft.Json;

namespace Material.Infrastructure.Credentials
{
    [DataContract]
    public class OAuth1Credentials : TokenCredentials
    {
        [DataMember(Name = "consumerKey")]
        [JsonProperty("consumerKey")]
        public string ConsumerKey { get; private set; }

        [DataMember(Name = "consumerSecret")]
        [JsonProperty("consumerSecret")]
        public string ConsumerSecret { get; private set; }

        [DataMember(Name = "oauth_token")]
        [JsonProperty("oauth_token")]
        public string OAuthToken { get; private set; }

        [DataMember(Name = "oauth_token_secret")]
        [JsonProperty("oauth_token_secret")]
        public string OAuthSecret { get; private set; }

        public string CallbackUrl { get; private set; }

        [DataMember(Name = "oauth_verifier")]
        [JsonProperty("oauth_verifier")]
        public string Verifier { get; private set; }

        [DataMember(Name = "parameterHandling")]
        [JsonProperty("parameterHandling")]
        public OAuthParameterTypeEnum ParameterHandling { get; private set; }

        [DataMember(Name = "x_auth_expired")]
        [JsonProperty("x_auth_expires")]
        private string _expires;


	    [JsonIgnore]
	    public override string ExpiresIn => _expires;

	    public override bool AreValidIntermediateCredentials =>
	        !string.IsNullOrEmpty(Verifier);

	    public OAuth1Credentials SetConsumerProperties(
	        string consumerKey,
	        string consumerSecret)
	    {
	        ConsumerKey = consumerKey;
	        ConsumerSecret = consumerSecret;

	        return this;
	    }

	    public OAuth1Credentials SetParameterHandling(
            OAuthParameterTypeEnum parameterHandling)
	    {
	        ParameterHandling = parameterHandling;

	        return this;
	    }

        public OAuth1Credentials SetCallbackUrl(string callbackUrl)
        {
            CallbackUrl = callbackUrl;

            return this;
        }

        [JsonIgnore]
        public override bool HasValidPublicKey => 
            !(string.IsNullOrEmpty(ConsumerKey) || 
              string.IsNullOrEmpty(ConsumerSecret));
    }
}
