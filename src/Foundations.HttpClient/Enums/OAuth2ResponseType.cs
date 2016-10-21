using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum OAuth2ResponseType
    {
        [Description("code")]
        Code,
        [Description("token")]
        Token
    }
}
