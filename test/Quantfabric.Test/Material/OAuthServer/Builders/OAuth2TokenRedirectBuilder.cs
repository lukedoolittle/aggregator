using System;
using System.Collections.Generic;
using Foundations;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.OAuthServer.Builders
{
    public class OAuth2TokenRedirectBuilder : IRedirectUriBuilder<OAuth2Credentials>
    {
        public Uri BuildRedirectUri(
            Uri redirectUri,
            OAuth2Credentials credentials, 
            Dictionary<string, string> additionalParameters)
        {
            var querystring = new HttpValueCollection();

            foreach (var parameter in additionalParameters)
            {
                querystring.Add(parameter.Key, parameter.Value);
            }

            querystring.Add("access_token", credentials.AccessToken);
            querystring.Add("expires_in", credentials.ExpiresIn);

            var builder = new UriBuilder(redirectUri);

            builder.Query += querystring.ToString();

            return builder.Uri;
        }
    }
}
