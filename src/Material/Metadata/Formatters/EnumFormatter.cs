using System;
using Foundations.Extensions;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EnumFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return (parameter as Enum).EnumToString();
        }
    }
}
