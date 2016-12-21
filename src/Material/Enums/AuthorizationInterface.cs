using Foundations.Attributes;

namespace Material.Enums
{
    public enum AuthorizationInterface
    {
        [Description("notSpecified")]NotSpecified,
        [Description("embedded")] Embedded,
        [Description("dedicated")] Dedicated,
        [Description("secureEmbedded")] SecureEmbedded,
    }
}
