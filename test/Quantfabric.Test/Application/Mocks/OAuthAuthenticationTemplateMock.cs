using System;
using System.Threading.Tasks;
using Material.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Application.Mocks
{
    public class OAuthAuthenticationTemplateMock<TCredentials> : 
        MockBase<IOAuthAuthenticationTemplate<TCredentials>>, 
        IOAuthAuthenticationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<TCredentials> IOAuthAuthenticationTemplate<TCredentials>.GetAccessTokenCredentials()
        {
            throw new NotImplementedException();
        }
    }
}
