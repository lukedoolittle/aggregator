using BatmansBelt.Metadata;

namespace Aggregator.Framework.Enums
{
    public enum GrantTypeEnum
    {
        [Description("authorization_code")] AuthCode,
        [Description("refresh_token")] RefreshToken
    }
}
