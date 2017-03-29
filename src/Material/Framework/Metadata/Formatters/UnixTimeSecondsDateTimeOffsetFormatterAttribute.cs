using System;
using System.Globalization;
using Material.Contracts;
using Material.Framework.Extensions;

namespace Material.Framework.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UnixTimeSecondsDateTimeOffsetFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((DateTimeOffset?) parameter)?.ToUnixTimeSeconds()
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}
