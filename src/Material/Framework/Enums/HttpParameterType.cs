using Material.Framework.Metadata;

namespace Material.Framework.Enums
{
    public enum HttpParameterType
    {
        [Description("header")]
        Header,
        [Description("querystring")]
        Querystring,
        [Description("body")]
        Body,
        [Description("unspecified")]
        Unspecified
    }
}
