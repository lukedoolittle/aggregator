using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations.Serialization;
using Newtonsoft.Json.Linq;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Aggregator.Infrastructure.Clients
{
    public class BluetoothClient<TRequest> : IRequestClient
        where TRequest : BluetoothRequest, new()
    {
        private readonly IBluetoothAdapter _manager;
        private readonly BluetoothCredentials _credentials;

        public BluetoothClient(
            IBluetoothAdapter manager, 
            BluetoothCredentials credentials)
        {
            _manager = manager;
            _credentials = credentials;
        }

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            var request = new TRequest();

            var result = await _manager.GetCharacteristicValue(
                _credentials.DeviceAddress,
                request.Service.AssignedNumber,
                request.Characteristic.AssignedNumber)
                .ConfigureAwait(false);

            var response = new Tuple<DateTimeOffset, JObject>(
                DateTimeOffset.Now, 
                request.CharacteristicConverter(result).AsJObject());
            return new List<Tuple<DateTimeOffset, JObject>> { response };
        }
    }
}