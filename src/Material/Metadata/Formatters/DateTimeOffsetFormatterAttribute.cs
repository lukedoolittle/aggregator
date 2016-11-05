using System;
using System.Globalization;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DateTimeOffsetFormatterAttribute : Attribute, IParameterFormatter
    {
        public string Value { get; }

        public DateTimeOffsetFormatterAttribute(string value)
        {
            Value = value;
        }

        public string FormatAsString(object parameter)
        {
            return ((DateTimeOffset?)parameter)
                ?.ToString(
                    Value, 
                    CultureInfo.InvariantCulture);
        }
    }
}
