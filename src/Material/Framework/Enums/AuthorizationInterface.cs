using Material.Framework.Metadata;

namespace Material.Framework.Enums
{
    public enum AuthorizationInterface
    {
        [Description("notSpecified")]NotSpecified,
        [Description("embedded")] Embedded,
        [Description("dedicated")] Dedicated,
        [Description("secureEmbedded")] SecureEmbedded,
    }
}
