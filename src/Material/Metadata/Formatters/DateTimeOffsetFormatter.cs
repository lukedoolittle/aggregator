using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Material.Contracts;

namespace Material.Metadata.Formatters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DateTimeOffsetFormatter : Attribute, IParameterFormatter
    {
        private readonly string _formatString;

        public DateTimeOffsetFormatter(string formatString)
        {
            _formatString = formatString;
        }

        public string FormatAsString(object parameter)
        {
            return ((DateTimeOffset)parameter).ToString(
                _formatString, 
                CultureInfo.InvariantCulture);
        }
    }
}
