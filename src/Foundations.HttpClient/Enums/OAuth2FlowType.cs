using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum OAuth2FlowType
    {
        [Description("accessCode")]
        AccessCode,
        [Description("implicit")]
        Implicit
    }
}
