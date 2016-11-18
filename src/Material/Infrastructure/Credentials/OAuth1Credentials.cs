using System.Runtime.Serialization;
using Foundations.HttpClient.Enums;

namespace Material.Infrastructure.Credentials
{
    [DataContract]
    public class OAuth1Credentials : TokenCredentials
    {
        [DataMember(Name = "consumerKey")]
        public string ConsumerKey { get; private set; }

        [DataMember(Name = "consumerSecret")]
        public string ConsumerSecret { get; private set; }

        [DataMember(Name = "oauth_token")]
        public string OAuthToken { get; private set; }

        [DataMember(Name = "oauth_token_secret")]
        public string OAuthSecret { get; private set; }

        [DataMember(Name = "oauth_callback_confirmed")]
        public bool CallbackConfirmed { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string CallbackUrl { get; private set; }

        [DataMember(Name = "oauth_verifier", EmitDefaultValue = false)]
        public string Verifier { get; private set; }

        [DataMember(Name = "parameterHandling")]
        public HttpParameterType ParameterHandling { get; private set; }

        //This will get assigned via deserialization
        [DataMember(Name = "x_auth_expired", EmitDefaultValue = false)]
#pragma warning disable 649
        private string _expires;
#pragma warning restore 649


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
            HttpParameterType parameterHandling)
	    {
	        ParameterHandling = parameterHandling;

	        return this;
	    }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public OAuth1Credentials SetCallbackUrl(string callbackUrl)
        {
            CallbackUrl = callbackUrl;

            return this;
        }
    }
}
