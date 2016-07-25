using System;
using System.Collections.Generic;
using System.Net;
using LightMock;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Aggregator.Test.Mocks
{
    using System.Threading.Tasks;

    public class OAuth2Mock : MockBase<IOAuth2Authentication>, IOAuth2Authentication
    {
        public OAuth2Mock SetReturnToken(OAuth2Credentials credentials)
        {
            _context.Arrange(a => 
                a.GetRefreshToken(
                    The<Uri>.IsAnyValue, 
                    The<string>.IsAnyValue, 
                    The<string>.IsAnyValue, 
                    The<string>.IsAnyValue,
                    The<Dictionary<HttpRequestHeader, string>>.IsAnyValue))
                .Returns(Task.FromResult(credentials));

            return this;
        }

        public Uri GetAuthorizationUri(
            Uri authorizeUrl, 
            string clientId, 
            string scope, 
            Uri redirectUri, 
            string state,
            ResponseTypeEnum responseType,
            Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<OAuth2Credentials> GetRefreshToken(
            Uri accessUrl, 
            string clientId, 
            string clientSecret, 
            string refreshToken,
            Dictionary<HttpRequestHeader, string> headers)
        {
            return _invoker.Invoke(
                    a => a.GetRefreshToken(
                        accessUrl, 
                        clientId, 
                        clientSecret, 
                        refreshToken,
                        headers));
        }

        public Task<OAuth2Credentials> GetAccessToken(Uri accessUrl, string clientId, string clientSecret, Uri callbackUrl, string code, string scope,
            Dictionary<HttpRequestHeader, string> additionalHeaders)
        {
            throw new NotImplementedException();
        }
    }
}
