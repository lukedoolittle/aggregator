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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<LinkedinUpdate>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Linkedin))]        
	public partial class LinkedinUpdate : OAuthRequest
	{
			public override String Host => "https://api.linkedin.com";
		public override String SinglePath => "/v1/people/~/network/updates";
		public override String RequestFilterKey => "";
		public override String ResponseFilterKey => "???";
		public override String HttpMethod => "GET";
		public override String PayloadProperty => "values";
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
		public override Dictionary<String,String> AdditionalQuerystringParameters => new Dictionary<String,String>{{"format", "json"},{"modified", "new"}};
	}
}
