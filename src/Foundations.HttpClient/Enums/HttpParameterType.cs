using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum HttpParameterType
    {
        [Description("header")]
        Header,
        [Description("querystring")]
        Querystring,
        [Description("body")]
        Body
    }
}
