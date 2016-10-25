using System;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BooleanFormatter : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((bool)parameter).ToString().ToLower();
        }
    }
}
