using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using Material.Framework.Collections;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth2CodeRedirectBuilder : 
        IRedirectUriBuilder<OAuth2Credentials>
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

            querystring.Add(
                OAuth2ResponseType.Code.EnumToString(), 
                credentials.Code);

            var builder = new UriBuilder(redirectUri);

            builder.Query += querystring.ToString();

            return builder.Uri;
        }
    }
}
