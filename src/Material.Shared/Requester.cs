using System.Threading.Tasks;
using Foundations.Serialization;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
#if __IOS__
using CoreLocation;
#endif
#if __MOBILE__
using Plugin.Geolocator;
using Aggregator.Infrastructure.Adapters;
using Robotics.Mobile.Core.Bluetooth.LE;
#endif

namespace Material
{
    public class OAuthRequester
    {
        private readonly OAuthProtectedResource _requester;
        private readonly string _userId;

        public OAuthRequester(OAuth1Credentials credentials)
        {
            _requester = new OAuthProtectedResource(
                credentials.ConsumerKey,
                credentials.ConsumerSecret,
                credentials.OAuthToken,
                credentials.OAuthSecret,
                credentials.ParameterHandling);

            _userId = credentials.UserId;
        }

        public OAuthRequester(OAuth2Credentials credentials)
        {
            _requester = new OAuthProtectedResource(
                credentials.AccessToken,
                credentials.TokenName);

            _userId = credentials.UserId;
        }

        public async Task<TResponse> MakeOAuthRequest<TRequest, TResponse>(
            TRequest request = null)
            where TRequest : OAuthRequest, new()
        {
            if (request == null)
            {
                request = new TRequest();
            }

            request.AddUserIdParameter(_userId);

            var result = await _requester
                .ForProtectedResource(
                    request.Host,
                    request.Path,
                    request.HttpMethod,
                    request.Headers,
                    request.QuerystringParameters,
                    request.PathParameters)
                .ConfigureAwait(false);

            return result.AsEntity<TResponse>(false);
        }
    }

    public class Requester
    {
#if __ANDROID__
        public static async Task<string> MakeSMSRequest()
        {
            var result = await new AndroidSMSAdapter()
                .GetAllSMS(null)
                .ConfigureAwait(false);

            return result.AsJson(false);
        }
#endif

#if __MOBILE__
        public static async Task<string> MakeGPSRequest()
        {
#if __IOS__
            new CLLocationManager().RequestAlwaysAuthorization();
            CrossGeolocator.Current.AllowsBackgroundUpdates = true;
#endif
            var result = await new GPSAdapter(CrossGeolocator.Current)
                .GetPosition()
                .ConfigureAwait(false);

            return result.First().Item2.AsJson(false);
        }

        public static async Task<string> MakeBluetoothRequest<TRequest>(
            BluetoothCredentials credentials)
            where TRequest : BluetoothRequest, new()
        {
#if __IOS__
            var adapter = Adapter.Current;
#else
            var adapter = new Adapter();
#endif
            var request = new TRequest();

            var result = await new BluetoothAdapter(adapter)
                .GetCharacteristicValue(
                    credentials.DeviceAddress,
                    request.ServiceGuid,
                    request.CharacteristicGuid)
                .ConfigureAwait(false);

            return result.Item2.AsJson(false);
        }
#endif

    }
}
