using System;
using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2ClientCredentials : IAuthenticatorParameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public HttpRequestHeader HeaderName => HttpRequestHeader.Authorization;
        public string Name => HeaderName.ToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Header;

        public OAuth2ClientCredentials(
            string clientId, 
            string clientSecret)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));

            var key = StringExtensions.Concatenate(
                clientId,
                clientSecret,
                ":").Utf8ToBase64String();

            Value = StringExtensions.Concatenate(
                OAuth2Parameter.BasicHeader.EnumToString(),
                key,
                " ");
        }
    }
}
