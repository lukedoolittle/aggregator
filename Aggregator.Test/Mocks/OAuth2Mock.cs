using System;
using System.Collections.Generic;
using LightMock;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Test.Helpers.Mocks;

namespace Aggregator.Test.Mocks
{
    using System.Threading.Tasks;

    public class OAuth2Mock : MockBase<IOAuth2>, IOAuth2
    {
        public OAuth2Mock SetReturnToken(OAuth2Credentials credentials)
        {
            _context.Arrange(a => 
                a.GetRefreshToken(
                    The<Uri>.IsAnyValue, 
                    The<string>.IsAnyValue, 
                    The<string>.IsAnyValue, 
                    The<string>.IsAnyValue, 
                    The<bool>.IsAnyValue))
                .Returns(Task.FromResult(credentials));

            return this;
        }


        public Uri GetAuthorizationPath(
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
            bool hasBasicAuthorization)
        {
            return _invoker.Invoke(
                    a => a.GetRefreshToken(
                        accessUrl, 
                        clientId, 
                        clientSecret, 
                        refreshToken, 
                        hasBasicAuthorization));
        }

        public Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl, 
            string clientId, 
            string clientSecret, 
            Uri callbackUrl, 
            string code, 
            string scope,
            bool hasBasicAuthorization)
        {
            throw new NotImplementedException();
        }
    }
}
