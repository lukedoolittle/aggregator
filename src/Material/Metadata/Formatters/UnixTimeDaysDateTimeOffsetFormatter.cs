using System;
using System.Globalization;
using Foundations.Extensions;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UnixTimeDaysDateTimeOffsetFormatter : Attribute, IParameterFormatter
    {
        public string FormatAsString(object parameter)
        {
            return ((DateTime)parameter)
                .ToUnixTimeDays()
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}
