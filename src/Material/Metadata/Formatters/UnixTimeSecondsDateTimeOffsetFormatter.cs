using System;
using System.Globalization;
using Foundations.Extensions;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UnixTimeSecondsDateTimeOffsetFormatter : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((DateTimeOffset?) parameter)?.ToUnixTimeSeconds()
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}
