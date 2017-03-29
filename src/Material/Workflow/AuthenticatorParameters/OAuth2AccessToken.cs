using System;
using System.Net;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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
