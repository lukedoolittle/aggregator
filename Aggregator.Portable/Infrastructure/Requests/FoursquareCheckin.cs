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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<FoursquareCheckin>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Foursquare))]        
	public partial class FoursquareCheckin : OAuthRequest
	{
			public override String Host => "https://api.foursquare.com";
		public override String SinglePath => "/v2/users/self/checkins";
		public override String RequestFilterKey => "afterTimestamp";
		public override String ResponseFilterKey => "createdAt";
		public override String HttpMethod => "GET";
		public override String PayloadProperty => "response.checkins.items";
		public override TimestampOptions ResponseTimestamp => new TimestampOptions("createdAt", null, "timeZoneOffset", null);
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
		public override Dictionary<String,String> AdditionalQuerystringParameters => new Dictionary<String,String>{{"v", "20140806"},{"m", "foursquare"}};
	}
}
