using System;
using System.Globalization;
using Foundations.Extensions;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UnixTimeSecondsDateTimeFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((DateTime?) parameter)?.ToUnixTimeSeconds()
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}
