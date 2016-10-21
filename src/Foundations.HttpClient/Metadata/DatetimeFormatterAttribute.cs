using System;

namespace Foundations.HttpClient.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DateTimeFormatterAttribute : Attribute
    {
        public string Formatter { get; }

        public DateTimeFormatterAttribute(string formatter)
        {
            Formatter = formatter;
        }
    }
}
