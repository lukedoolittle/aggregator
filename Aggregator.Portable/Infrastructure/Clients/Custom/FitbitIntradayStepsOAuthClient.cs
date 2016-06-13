using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class FitbitIntradayStepsOAuthClient :
        FitbitIntradayOAuthClient<FitbitIntradaySteps>
    {      
        public FitbitIntradayStepsOAuthClient(IOAuth oauthRequest) :
            base(oauthRequest)
        {
        }
    }
}
