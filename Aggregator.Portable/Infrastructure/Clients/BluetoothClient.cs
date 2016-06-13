using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class BluetoothClient<TRequest> : IRequestClient
        where TRequest : BluetoothRequest, new()
    {
        private readonly IBluetoothManager _manager;
        private readonly BluetoothCredentials _credentials;

        public BluetoothClient(
            IBluetoothManager manager, 
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
                request.ServiceGuid,
                request.CharacteristicGuid)
                .ConfigureAwait(false);

            return new List<Tuple<DateTimeOffset, JObject>> { result };
        }
    }
}