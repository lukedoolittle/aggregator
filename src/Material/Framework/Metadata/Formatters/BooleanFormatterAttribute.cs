using System;
using Material.Contracts;

namespace Material.Framework.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BooleanFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((bool?) parameter)?.ToString().ToLower();
        }
    }
}
