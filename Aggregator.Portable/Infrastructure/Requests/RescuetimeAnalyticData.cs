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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.RescuetimeOAuthClient))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Rescuetime))]        
	public partial class RescuetimeAnalyticData : OAuthRequest
	{
			public override String Host => "https://www.rescuetime.com/";
		public override String SinglePath => "/api/oauth/data";
		public override String RequestFilterKey => "restrict_begin";
		public override String ResponseFilterKey => "Date";
		public override String HttpMethod => "GET";
		public override String PayloadProperty => "";
		public override TimestampOptions ResponseTimestamp => new TimestampOptions("Date", "yyyy-MM-ddTHH:mm:ss", null, null);
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
		public override Dictionary<String,String> AdditionalQuerystringParameters => new Dictionary<String,String>{{"format", "json"},{"interval", "hour"},{"perspective", "interval"},{"restrict_kind", "efficiency"}};
	}
}
