using System;
using System.Collections.Generic;
using System.Text;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Application.Mocks
{
    public class OAuthFactoryMock : MockBase<IOAuthFactory>, IOAuthFactory
    {
        public IOAuthProtectedResource GetOAuth(OAuth2Credentials credentials)
        {
            throw new NotImplementedException();
        }

        public IOAuthProtectedResource GetOAuth(OAuth1Credentials credentials)
        {
            throw new NotImplementedException();
        }

        public IOAuth1Authentication GetOAuth1()
        {
            throw new NotImplementedException();
        }

        public IOAuth2Authentication GetOAuth2()
        {
            throw new NotImplementedException();
        }
    }
}
