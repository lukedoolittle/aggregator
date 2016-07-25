using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Commands;
using Foundations.Serialization;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    using System.Threading.Tasks;

    public class BluetoothAuthenticationTask<TService> : ITask
        where TService : ResourceProvider
    {
        private readonly IBluetoothAuthorizerUI _authorizerUI;
        private readonly bool _isUpdate;
        private readonly ICommandSender _sender;
        private readonly Guid _aggregateId;
        private readonly int _originalVersion;

        public BluetoothAuthenticationTask(
            IBluetoothAuthorizerUI authorizerUI,
            ICommandSender sender, 
            Guid aggregateId, 
            int originalVersion, 
            bool isUpdate)
        {
            _authorizerUI = authorizerUI;
            _sender = sender;
            _aggregateId = aggregateId;
            _originalVersion = originalVersion;
            _isUpdate = isUpdate;
        }

        async Task ITask.Execute(object parameter = null)
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

        public Task<BluetoothCredentials> GetCredentials()
        {
            return _authorizerUI.GetDeviceUuid();
        }
    }
}
