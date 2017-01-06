using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace CodeGen
{
    public class BoxedOAuth1ResourceProvider
    {
        public string Name { get; }
        public string Comments { get; }
        public Uri RequestUrl { get; }
        public Uri AuthorizationUrl { get; }
        public Uri TokenUrl { get; }
        public HttpParameterType ParameterType { get; }
        public bool SupportsCustomUrlScheme { get; }

        public BoxedOAuth1ResourceProvider(
            string name,
            string comments,
            SecurityDefinition security) : this(
                name,
                comments,
                security.RequestUrl,
                security.AuthorizationUrl,
                security.TokenUrl,
                security.ParameterLocation,
                security.CustomSchemeSupport)
        {
        }

        public BoxedOAuth1ResourceProvider(
            string name,
            string comments,
            string requestUrl,
            string authorizationUrl,
            string tokenUrl,
            string parameterType,
            bool supportsCustomUrlScheme)
        {
            Name = name;
            Comments = comments;
            RequestUrl = new Uri(requestUrl);
            AuthorizationUrl = new Uri(authorizationUrl);
            TokenUrl = new Uri(tokenUrl);
            ParameterType = parameterType.StringToEnum<HttpParameterType>();
            SupportsCustomUrlScheme = supportsCustomUrlScheme;
        }
    }
}
