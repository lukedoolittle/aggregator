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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.SMSClient))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.SMS))]        
	public partial class SMSTextMessage : OnboardRequest
	{
			public override String PayloadProperty => "";
		public override String ResponseFilterKey => "date";
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
