using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NameAttribute : Attribute
    {
        public string Value { get; }

        public NameAttribute(string value)
        {
            Value = value;
        }
    }
}
