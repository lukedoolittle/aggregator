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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.FitbitIntradayHeartRateOAuthClient))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Fitbit))]        
	public partial class FitbitIntradayHeartRate : OAuthRequest
	{
			public override String Host => "https://api.fitbit.com";
		public override String SinglePath => "1/user/-/activities/heart/date/{startdate}/{enddate}/1min/time/{starttime}/{endtime}.json";
		public override String BulkPath => "1/user/-/activities/heart/date/{date}/1d/1min.json";
		public override String HttpMethod => "GET";
		public override String RequestFilterKey => "";
		public override String ResponseFilterKey => "dateTime";
		public override String PayloadProperty => "activities-heart-intraday.dataset";
		public override TimestampOptions ResponseTimestamp => new TimestampOptions("dateTime", "yyyy-MM-dd HH:mm:ss zzz", null, null);
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
