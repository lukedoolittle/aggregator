using Material.Framework.Metadata;

namespace Material.Framework.Enums
{
    public enum OAuth2ResponseType
    {
        [Description("code")]
        Code,
        [Description("token")]
        Token,
        [Description("id_token")]
        IdToken,
        [Description("id_token token")]
        IdTokenToken,
        [Description("id_token code")]
        IdTokenCode
    }
}
