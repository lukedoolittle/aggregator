using Aggregator.Framework.Contracts;
using Material.Contracts;
using Material.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class FitbitIntradayStepsOAuthClient :
        FitbitIntradayOAuthClient<FitbitIntradaySteps>
    {      
        public FitbitIntradayStepsOAuthClient(IOAuthProtectedResource oauthRequest) :
            base(oauthRequest)
        {
        }
    }
}
