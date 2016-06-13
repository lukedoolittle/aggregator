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
	[Aggregator.Framework.Metadata.ClientType(typeof(Aggregator.Infrastructure.Clients.BluetoothClient<MioHeartRate>))]        
	[Aggregator.Framework.Metadata.ServiceType(typeof(Aggregator.Infrastructure.Services.Mioalpha))]        
	public partial class MioHeartRate : BluetoothRequest
	{
			public override String PayloadProperty => "";
		public override String ServiceUuid => "180D";
		public override String CharacteristicUuid => "2A37";
		public override String ResponseFilterKey => "timestamp";
		public override PollingInterval PollingInterval => new PollingInterval(1000, 5000, 10000);
	}
}
