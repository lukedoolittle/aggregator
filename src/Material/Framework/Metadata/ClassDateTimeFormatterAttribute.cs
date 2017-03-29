using System;

namespace Material.Framework.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ClassDateTimeFormatterAttribute : Attribute
    {
        public string Formatter { get; }

        public ClassDateTimeFormatterAttribute(string formatter)
        {
            Formatter = formatter;
        }
    }
}
