using System;
using Material.Contracts;
using Material.Framework.Extensions;

namespace Material.Framework.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EnumFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return (parameter as Enum)?.EnumToString();
        }
    }
}
