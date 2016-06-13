using System.Collections.Generic;
using Newtonsoft.Json;
using Aggregator.Framework.Enums;

namespace Aggregator.Infrastructure.Credentials
{
	public class OAuth1Credentials : TokenCredentials
    {
        [JsonProperty("consumerKey")]
        public string ConsumerKey { get; private set; }

        [JsonProperty("consumerSecret")]
        public string ConsumerSecret { get; private set; }

        [JsonProperty("oauth_token")]
        public string OAuthToken { get; private set; }

        [JsonProperty("oauth_token_secret")]
        public string OAuthSecret { get; private set; }

        [JsonProperty("oauth_verifier")]
        public string Verifier { get; private set; }

        [JsonProperty("parameterHandling")]
        public OAuthParameterEnum ParameterHandling { get; set; }

	    public OAuth1Credentials SetConsumerProperties(
	        string consumerKey,
	        string consumerSecret)
	    {
	        ConsumerKey = consumerKey;
	        ConsumerSecret = consumerSecret;

	        return this;
	    }

	    public OAuth1Credentials MergeAdditionalParameters(
	        Dictionary<string, string> additionalQuerystringParameters)
	    {
	        foreach (var item in additionalQuerystringParameters)
	        {
	            if (!AdditionalTokenParameters.ContainsKey(item.Key))
	            {
	                AdditionalTokenParameters.Add(item.Key, item.Value);
	            }
	        }

	        return this;
	    }
        [JsonIgnore]
        public override bool HasValidProperties => 
            !(string.IsNullOrEmpty(ConsumerKey) || 
              string.IsNullOrEmpty(ConsumerSecret));

        [JsonIgnore]
        public override bool IsTokenExpired => false;
    }
}
