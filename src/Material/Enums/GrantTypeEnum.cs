using Foundations.Attributes;

namespace Material.Enums
{
    public enum GrantTypeEnum
    {
        [Description("authorization_code")] AuthCode,
        [Description("refresh_token")] RefreshToken
    }
}
