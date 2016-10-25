using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FormatAttribute : Attribute
    {
        public string Formatter { get; }

        public FormatAttribute(string formatter)
        {
            Formatter = formatter;
        }
    }
}
