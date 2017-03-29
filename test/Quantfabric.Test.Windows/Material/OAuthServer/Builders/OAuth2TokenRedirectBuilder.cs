using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using Material.Framework.Collections;

namespace Quantfabric.Test.Material.OAuthServer.Builders
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
