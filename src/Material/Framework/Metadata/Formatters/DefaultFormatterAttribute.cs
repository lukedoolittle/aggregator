using System;
using Material.Contracts;

namespace Material.Framework.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DefaultFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return parameter?.ToString();
        }
    }
}
