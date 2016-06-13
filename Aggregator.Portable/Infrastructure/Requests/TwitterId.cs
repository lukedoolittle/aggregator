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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<TwitterId>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Twitter))]        
	public partial class TwitterId : OAuthRequest
	{
			public override String Host => "https://api.twitter.com/";
		public override String SinglePath => "1.1/account/verify_credentials.json";
		public override String HttpMethod => "GET";
		public override String PayloadProperty => "id";
		public override String RequestFilterKey => "";
		public override String ResponseFilterKey => "";
	}
}
