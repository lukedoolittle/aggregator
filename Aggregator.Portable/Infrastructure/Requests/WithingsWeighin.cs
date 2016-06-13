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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.WithingsWeighinOAuthClient))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Withings))]        
	public partial class WithingsWeighin : OAuthRequest
	{
			public override String Host => "https://wbsapi.withings.net";
		public override String SinglePath => "/measure";
		public override String HttpMethod => "GET";
		public override String RequestFilterKey => "lastupdate";
		public override String ResponseFilterKey => "date";
		public override String PayloadProperty => "body.measuregrps";
		public override TimestampOptions ResponseTimestamp => new TimestampOptions("date", null, null, "0000");
		public override Dictionary<String,String> AdditionalQuerystringParameters => new Dictionary<String,String>{{"action", "getmeas"},{"userid", ""}};
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
