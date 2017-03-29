using System;
using System.Globalization;
using Material.Framework.Enums;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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
