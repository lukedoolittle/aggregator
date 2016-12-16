using System;
using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2AccessToken : IAuthenticatorParameter
    {
        public string Name { get; }
        public string Value { get; }
        public HttpParameterType Type { get; }

        public OAuth2AccessToken(
            string accessToken, 
            string accessTokenName)
        {
            if (string.Equals(
                accessTokenName, 
                OAuth2Parameter.BearerHeader.EnumToString(), 
                StringComparison.CurrentCultureIgnoreCase))
            {
                Name = HttpRequestHeader.Authorization.ToString();
                Value = StringExtensions.Concatenate(
                    OAuth2Parameter.BearerHeader.EnumToString(),
                    accessToken,
                    " ");
                Type = HttpParameterType.Header;
            }
            else
            {
                Name = accessTokenName;
                Value = accessToken;
                Type = HttpParameterType.Querystring;
            }
        }
    }
}
