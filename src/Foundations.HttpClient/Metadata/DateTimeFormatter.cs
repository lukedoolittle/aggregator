using System;

namespace Foundations.HttpClient.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatetimeFormatter : Attribute
    {
        public string Formatter { get; set; }

        public DatetimeFormatter(string formatter)
        {
            Formatter = formatter;
        }
    }
}
