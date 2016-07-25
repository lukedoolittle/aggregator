using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Aggregator.Test.Mocks
{
    public class RefreshTokenFacadeMock : 
        MockBase<IRefreshTokenFacade>, 
        IRefreshTokenFacade
    {
        public Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials<TResourceProvider>(OAuth2Credentials expiredCredentials,
            TResourceProvider provider = default(TResourceProvider)) where TResourceProvider : OAuth2ResourceProvider, new()
        {
            throw new NotImplementedException();
        }
    }
}
