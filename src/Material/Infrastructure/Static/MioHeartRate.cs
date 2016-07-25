using System;
using Material.Infrastructure.ProtectedResources;
using Material.Metadata;

namespace Material.Infrastructure.Requests
{       
	[ServiceType(typeof(Mioalpha))]        
	public partial class MioHeartRate : BluetoothRequest
	{
		public override String ServiceUuid => "180D";
		public override String CharacteristicUuid => "2A37";
	}
}
