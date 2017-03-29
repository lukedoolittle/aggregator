using System;
using System.Globalization;
using Material.Contracts;
using Material.Framework.Extensions;

namespace Material.Framework.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UnixTimeDaysDateTimeOffsetFormatterAttribute : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((DateTime?) parameter)?.ToUnixTimeDays()
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}
