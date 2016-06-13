using System;
using BatmansBelt.Serialization;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Commands;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    using System.Threading.Tasks;

    public class BluetoothAuthenticationTask<TService> : ITask
        where TService : Service
    {
        private readonly IBluetoothAuthorizer _authorizer;
        private readonly bool _isUpdate;
        private readonly ICommandSender _sender;
        private readonly Guid _aggregateId;
        private readonly int _originalVersion;

        public BluetoothAuthenticationTask(
            IBluetoothAuthorizer authorizer,
            ICommandSender sender, 
            Guid aggregateId, 
            int originalVersion, 
            bool isUpdate)
        {
            _authorizer = authorizer;
            _sender = sender;
            _aggregateId = aggregateId;
            _originalVersion = originalVersion;
            _isUpdate = isUpdate;
        }

        public async Task Execute(object parameter = null)
        {
            var credentials = await GetCredentials().ConfigureAwait(false);

            if (_isUpdate)
            {
                await _sender.Send(
                    new UpdateTokenCommand<TService>(
                        _aggregateId,
                        credentials.AsJObject(),
                        _originalVersion))
                    .ConfigureAwait(false);
            }
            else
            {
                await _sender.Send(
                    new CreateTokenCommand<TService>(
                        _aggregateId,
                        credentials.AsJObject(),
                        _originalVersion))
                    .ConfigureAwait(false);
            }
        }

        public async Task<BluetoothCredentials> GetCredentials()
        {
            var deviceAddress = await _authorizer
                .GetDeviceUuid()
                .ConfigureAwait(false);

            return new BluetoothCredentials(deviceAddress);
        }
    }
}
