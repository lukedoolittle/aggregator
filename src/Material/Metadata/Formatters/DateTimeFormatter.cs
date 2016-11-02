using System;
using System.Globalization;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DateTimeFormatter : Attribute, IParameterFormatter
    {
        private readonly string _formatString;

        public DateTimeFormatter(string formatString)
        {
            _formatString = formatString;
        }

        public string FormatAsString(object parameter)
        {
            return ((DateTime?)parameter)
                ?.ToString(
                    _formatString,
                    CultureInfo.InvariantCulture);
        }
    }
}
