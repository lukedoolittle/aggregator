using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum OAuthParameterTypeEnum
    {
        [Description("header")]
        Header,
        [Description("querystring")]
        Querystring,
        [Description("body")]
        Body
    }
}
