#if __MOBILE__
using System;
using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Framework;
using Material.Infrastructure.Bluetooth;
using Material.Infrastructure.Responses;
using Material.Contracts;

#if __ANDROID__
using Material.Permissions;
#endif

namespace Material.Bluetooth
{
    public class BluetoothRequester
    {
        /// <summary>
        /// Make a request for a single piece of data from a Bluetooth provider
        /// </summary>
        /// <typeparam name="TRequest">Request type for provider</typeparam>
        /// <param name="credentials">Credentials for provider</param>
        /// <returns>Resource from provider</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public Task<BluetoothResponse> MakeBluetoothRequestAsync<TRequest>(
            BluetoothCredentials credentials)
            where TRequest : BluetoothRequest, new()
        {
            return MakeBluetoothRequestAsync<TRequest>(credentials, true);
        }

        /// <summary>
        /// Make a request for a single piece of data from a Bluetooth provider
        /// </summary>
        /// <typeparam name="TRequest">Request type for provider</typeparam>
        /// <param name="credentials">Credentials for provider</param>
        /// <param name="skipBluetoothAuthorization">If true bypasses device bluetooth authorization</param>
        /// <returns>Resource from provider</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public async Task<BluetoothResponse> MakeBluetoothRequestAsync<TRequest>(
            BluetoothCredentials credentials,
            bool skipBluetoothAuthorization)
            where TRequest : BluetoothRequest, new()
        {
            var completionSource = new TaskCompletionSource<BluetoothResponse>();

            var subscriptionManager = await SubscribeToBluetoothRequest<TRequest>(
                    credentials,
                    response =>
                    {
                        if (!completionSource.Task.IsCompleted)
                        {
                            completionSource.SetResult(response);
                        }
                    }, 
                    skipBluetoothAuthorization)
                .ConfigureAwait(false);

            var result = await completionSource
                .Task
                .ConfigureAwait(false);
            subscriptionManager.Unsubscribe();

            return result;
        }

        /// <summary>
        /// Subscribe to a bluetooth characteristic feed
        /// </summary>
        /// <param name="credentials">Credentials for provider</param>
        /// <param name="callback"></param>
        /// <returns>The subscription handler</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public Task<ISubscriptionManager> SubscribeToBluetoothRequest<TRequest>(
            BluetoothCredentials credentials,
            Action<BluetoothResponse> callback)
            where TRequest : BluetoothRequest, new()
        {
            return SubscribeToBluetoothRequest<TRequest>(
                credentials, 
                callback, 
                true);
        }

        /// <summary>
        /// Subscribe to a bluetooth characteristic feed
        /// </summary>
        /// <param name="credentials">Credentials for provider</param>
        /// <param name="callback"></param>
        /// <param name="skipBluetoothAuthorization">If true bypasses device bluetooth authorization</param>
        /// <returns>The subscription handler</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public async Task<ISubscriptionManager> SubscribeToBluetoothRequest<TRequest>(
            BluetoothCredentials credentials,
            Action<BluetoothResponse> callback,
            bool skipBluetoothAuthorization)
            where TRequest : BluetoothRequest, new()
        {
            var request = new TRequest();

#if __ANDROID__
            if (!skipBluetoothAuthorization)
            {
                var authorizationResult = await new DeviceAuthorizationFacade()
                    .AuthorizeBluetooth()
                    .ConfigureAwait(false);
            }
#endif

            return await new BluetoothAdapter(Platform.Current.BluetoothAdapter)
                .SubscribeToCharacteristicValue(
                    new GattDefinition(
                        credentials.DeviceAddress,
                        request.Service.AssignedNumber,
                        request.Characteristic.AssignedNumber),
                    bytes =>
                    {
                        callback(new BluetoothResponse
                        {
                            Reading = request.CharacteristicConverter(bytes),
                            Timestamp = DateTimeOffset.Now
                        });
                    }).ConfigureAwait(false);
        }
    }
}
#endif
