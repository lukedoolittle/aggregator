using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum GrantTypeEnum
    {
        [Description("authorization_code")]
        AuthCode,
        [Description("refresh_token")]
        RefreshToken,
        [Description("client_credentials")]
        ClientCredentials,
        [Description("urn:ietf:params:oauth:grant-type:jwt-bearer")]
        JsonWebToken
    }
}
