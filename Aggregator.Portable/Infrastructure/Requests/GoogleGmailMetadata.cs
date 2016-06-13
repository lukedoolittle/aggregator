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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.OAuthClient<GoogleGmailMetadata>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Google))]        
	public partial class GoogleGmailMetadata : OAuthRequest
	{
			public override String Host => "https://www.googleapis.com";
		public override String SinglePath => "/gmail/v1/users/me/messages";
		public override String RequestFilterKey => "newer_than";
		public override String ResponseFilterKey => "";
		public override String PayloadProperty => "messages";
		public override String HttpMethod => "GET";
	}
}
