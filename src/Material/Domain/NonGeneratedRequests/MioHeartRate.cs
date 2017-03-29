using System;
using Material.Domain.Bluetooth;
using Material.Domain.Core;
using Material.Framework.Metadata;
using Material.Domain.ResourceProviders;

namespace Material.Domain.Requests
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
