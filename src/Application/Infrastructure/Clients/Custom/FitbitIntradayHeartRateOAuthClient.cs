using Aggregator.Framework.Contracts;
using Material.Contracts;
using Material.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class FitbitIntradayHeartRateOAuthClient :
        FitbitIntradayOAuthClient<FitbitIntradayHeartRate>
    {
        public FitbitIntradayHeartRateOAuthClient(IOAuthProtectedResource oauthRequest) :
            base(oauthRequest)
        {
        }
    }
}
