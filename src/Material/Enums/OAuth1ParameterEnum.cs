using Foundations.Attributes;

namespace Material.Enums
{
    public enum OAuth1ParameterEnum
    {
        [Description("oauth_token")]
        OAuthToken,
        [Description("oauth_token_secret")]
        OAuthTokenSecret,
        [Description("denied")]
        Error
    }
}
