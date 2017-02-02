using System;
using System.Globalization;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class MicrosoftDate : IAuthenticatorParameter
    {
        public string Name => "x-ms-date";
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Header;

        public MicrosoftDate(DateTimeOffset timestamp)
        {
            Value = timestamp
                .UtcDateTime
                .ToString(
                    "R", 
                    CultureInfo.InvariantCulture);
        }

        public MicrosoftDate() :
            this(DateTime.UtcNow)
        { }
    }
}
