using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Name : Attribute
    {
        public string Value { get; set; }

        public Name(string value)
        {
            Value = value;
        }
    }
}
