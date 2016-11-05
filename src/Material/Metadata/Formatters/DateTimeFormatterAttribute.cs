using System;
using System.Globalization;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DateTimeFormatterAttribute : Attribute, IParameterFormatter
    {
        public string Value { get; }

        public DateTimeFormatterAttribute(string value)
        {
            Value = value;
        }

        public string FormatAsString(object parameter)
        {
            return ((DateTime?)parameter)
                ?.ToString(
                    Value,
                    CultureInfo.InvariantCulture);
        }
    }
}
