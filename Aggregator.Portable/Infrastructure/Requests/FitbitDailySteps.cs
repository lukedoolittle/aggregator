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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<FitbitDailySteps>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Fitbit))]        
	public partial class FitbitDailySteps : OAuthRequest
	{
			public override String Host => "https://api.fitbit.com";
		public override String SinglePath => "/1/user/-/activities/steps/date/today/1d.json";
		public override String HttpMethod => "GET";
		public override String RequestFilterKey => "";
		public override String ResponseFilterKey => "";
		public override String PayloadProperty => "activities-log-steps";
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
