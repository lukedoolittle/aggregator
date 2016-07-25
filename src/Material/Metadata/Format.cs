using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Format : Attribute
    {
        public string Formatter { get; set; }

        public Format(string formatter)
        {
            Formatter = formatter;
        }
    }
}
