using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class FitbitIntradayHeartRateOAuthClient :
        FitbitIntradayOAuthClient<FitbitIntradayHeartRate>
    {
        public FitbitIntradayHeartRateOAuthClient(IOAuth oauthRequest) :
            base(oauthRequest)
        {
        }
    }
}
