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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<FatsecretMeal>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Fatsecret))]        
	public partial class FatsecretMeal : OAuthRequest
	{
			public override String Host => "http://platform.fatsecret.com";
		public override String SinglePath => "/rest/server.api";
		public override String HttpMethod => "GET";
		public override String RequestFilterKey => "startdate";
		public override String ResponseFilterKey => "";
		public override String PayloadProperty => "food_entries";
		public override TimestampOptions ResponseTimestamp => new TimestampOptions("date_int", null, null, null);
		public override Dictionary<String,String> AdditionalQuerystringParameters => new Dictionary<String,String>{{"action", "getmeas"},{"userid", "NEEDSTOBEFILLEDIN"}};
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
