using Foundations.Attributes;

namespace Material.Enums
{
    public enum AuthorizationInterface
    {
        [Description("embedded")] Embedded,
        [Description("dedicated")] Dedicated,
        [Description("secureEmbedded")] SecureEmbedded,
        [Description("notSpecified")] NotSpecified
    }
}
