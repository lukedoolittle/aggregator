using Foundations.Attributes;

namespace Material.Enums
{
    public enum OAuth2ParameterEnum
    {
        [Description("oauth_token")]
        OAuthToken,
        [Description("redirect_uri")]
        RedirectUri,
        [Description("client_id")]
        ClientId,
        [Description("client_secret")]
        ClientSecret,
        [Description("scope")]
        Scope,
        [Description("grant_type")]
        GrantType,
        [Description("response_type")]
        ResponseType,
        [Description("state")]
        State,
        [Description("nonce")]
        Nonce,
        [Description("Bearer")]
        BearerHeader,
        [Description("Basic")]
        BasicHeader,
        [Description("error")]
        Error
    }
}
