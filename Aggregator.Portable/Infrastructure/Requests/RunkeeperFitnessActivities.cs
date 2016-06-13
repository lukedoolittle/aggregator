/*
 * WARNING: This is generated code. Any changes you make will
 * be overwritten. If you wish to modify this class create a
 * partial definition in a seperate file.
 *
 */
using System;
using System.Collections.Generic;
using Aggregator.Domain.Write;
using Newtonsoft.Json.Linq;

namespace Aggregator.Infrastructure.Requests
{
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<RunkeeperFitnessActivities>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Runkeeper))]        
	public partial class RunkeeperFitnessActivities : OAuthRequest
	{
			public override String Host => "https://api.runkeeper.com";
		public override String SinglePath => "/fitnessActivities";
		public override String HttpMethod => "GET";
		public override Dictionary<String,String> Headers => new Dictionary<String,String>{{"Accept", "application/vnd.com.runkeeper.FitnessActivityFeed+json"}};
		public override String RequestFilterKey => "noEarlierThan";
		public override String ResponseFilterKey => "start_time";
		public override String PayloadProperty => "items";
		public override TimestampOptions ResponseTimestamp => new TimestampOptions("start_time", "ddd, d MMM yyyy HH:mm:ss", "utc_offset", null);
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
