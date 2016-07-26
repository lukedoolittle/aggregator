using System;
using Material.Infrastructure.Bluetooth;
using Material.Infrastructure.ProtectedResources;
using Material.Metadata;

namespace Material.Infrastructure.Requests
{       
	[ServiceType(typeof(Mioalpha))]        
	public partial class MioHeartRate : BluetoothRequest
	{
	    public override BluetoothSpecification Characteristic => 
            BluetoothCharacteristics.HeartRateMeasurement;

	    public override Func<byte[], string> CharacteristicConverter =>
	        BluetoothCharacteristicConverters.DecodeHeartRateCharacteristic;

	    public override BluetoothSpecification Service => 
            BluetoothServices.HeartRate;
	}
}
