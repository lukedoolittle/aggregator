using Material.Framework.Metadata;

namespace Material.Framework.Enums
{
    public enum ContentTypeEncoding
    {
        [Description("utf-8")]
        UTF8,
        [Description("utf-16be")]
        UTF16BigEndian,
        [Description("utf-16le")]
        UTF16LittleEndian
    }
}
