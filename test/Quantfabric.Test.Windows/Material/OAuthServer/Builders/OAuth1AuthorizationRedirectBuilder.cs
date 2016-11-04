using System;
using System.Collections.Generic;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth1AuthorizationRedirectBuilder : 
        IRedirectUriBuilder<OAuth1Credentials>
    {
        public Uri BuildRedirectUri(
            Uri redirectUri, 
            OAuth1Credentials credentials, 
            Dictionary<string, string> additionalParameters)
        {
            var querystring = new HttpValueCollection();

            foreach (var parameter in additionalParameters)
            {
                querystring.Add(parameter.Key, parameter.Value);
            }

            querystring.Add(
                OAuth1Parameter.Verifier.EnumToString(), 
                credentials.Verifier);

            querystring.Add(
                OAuth1Parameter.OAuthToken.EnumToString(),
                credentials.OAuthToken);

            var builder = new UriBuilder(redirectUri);

            builder.Query += querystring.ToString();

            return builder.Uri;
        }
    }
}
