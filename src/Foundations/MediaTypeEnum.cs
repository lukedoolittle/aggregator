using Foundations.Attributes;

namespace Foundations.Http
{
    public enum MediaTypeEnum
    {
        [Description("application/json")]
        Json,
        [Description("text/plain")]
        Text,
        [Description("text/html")]
        Html,
        [Description("text/xml")]
        Xml,
        [Description("application/x-www-form-urlencoded")]
        Form
    }
}
