using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum OAuth1ParameterEnum
    {
        [Description("oauth_consumer_key")]
        ConsumerKey,
        [Description("oauth_consumer_secret")]
        ConsumerSecret,
        [Description("oauth_timestamp")]
        Timestamp,
        [Description("oauth_version")]
        Version,
        [Description("oauth_signature_method")]
        SignatureMethod,
        [Description("oauth_signature")]
        Signature,
        [Description("oauth_token")]
        OAuthToken,
        [Description("oauth_token_secret")]
        OAuthTokenSecret,
        [Description("oauth_verifier")]
        Verifier,
        [Description("oauth_nonce")]
        Nonce,
        [Description("oauth_callback")]
        Callback,
        [Description("denied")]
        Error
    }
}
