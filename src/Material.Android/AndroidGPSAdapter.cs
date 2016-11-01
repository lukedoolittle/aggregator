using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Foundations.Extensions;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Requests;

namespace Material
{
    public class AndroidGPSAdapter : 
        Java.Lang.Object, 
        IGPSAdapter,
        ILocationListener
    {
        private readonly object _syncLock = new object();
        private readonly LocationManager _locationManager;
        private readonly ConcurrentBag<string> _locationProviders =
            new ConcurrentBag<string>();
        private readonly int _timeoutInMs;
        private readonly float _desiredAccuracyInM;

        private TaskCompletionSource<GPSResponse> _completionSource;
        private bool _isListening = false;
        private Timer _timer;

        public AndroidGPSAdapter(
            int gpsTimeoutInMs = 30000,
            float desiredAccuracyInMeters = 50)
        {
            _timeoutInMs = gpsTimeoutInMs;
            _desiredAccuracyInM = desiredAccuracyInMeters;

            _locationManager = (LocationManager)Application
                .Context
                .GetSystemService(
                    Context.LocationService);

            var acceptableLocationProviders = _locationManager
                .GetProviders(enabledOnly: false);

            foreach (var provider in acceptableLocationProviders)
            {
                _locationProviders.Add(provider);
            }
        }

        public Task<GPSResponse> GetPositionAsync()
        {
            if (_isListening)
            {
                return _completionSource.Task;
            }

            if (!_locationProviders.Any())
            {
                throw new LocationException(
                    StringResources.GPSDisabledConnectivityException);
            }

            lock (_syncLock)
            {
                _completionSource =
                    new TaskCompletionSource<GPSResponse>();
                _isListening = true;
                _timer = new Timer(_timeoutInMs);
                _timer.Elapsed += (sender, args) =>
                {
                    Cleanup();
                    throw new LocationException(
                        StringResources.GPSTimeoutConnectivityException);
                };
                _timer.Start();


                var looper = Looper.MyLooper() ?? Looper.MainLooper;

                foreach (var provider in _locationProviders)
                {
                    _locationManager.RequestLocationUpdates(
                        provider,
                        _timeoutInMs,
                        _desiredAccuracyInM,
                        this,
                        looper);
                }

                return _completionSource.Task;
            }
        }

        private void Cleanup()
        {
            _timer.Stop();
            _timer = null;
            _completionSource = null;
            _locationManager.RemoveUpdates(this);
            _isListening = false;
        }

        public void OnLocationChanged(Location location)
        {
            lock (_syncLock)
            {
                if (_isListening && 
                    location.Accuracy <= _desiredAccuracyInM)
                {
                    var response = new GPSResponse();
                    if (location.HasAccuracy)
                        response.Accuracy = location.Accuracy;
                    if (location.HasAltitude)
                        response.Altitude = location.Altitude;
                    if (location.HasBearing)
                        response.Heading = location.Bearing;
                    if (location.HasSpeed)
                        response.Speed = location.Speed;

                    response.Longitude = location.Longitude;
                    response.Latitude = location.Latitude;
                    response.Timestamp = location.Time.FromUnixTimeMilliseconds();
                    _completionSource.SetResult(response);
                    Cleanup();
                }
            }
        }

        public void OnProviderDisabled(string provider)
        {
            if (provider == LocationManager.PassiveProvider)
                return;

            _locationProviders.TryTake(out provider);

            if (!_locationProviders.Any())
            {
                throw new LocationException(
                    StringResources.GPSDisabledConnectivityException);
            }
        }

        public void OnProviderEnabled(string provider)
        {
            if (provider == LocationManager.PassiveProvider)
                return;

            _locationProviders.Add(provider);
        }

        public void OnStatusChanged(
            string provider, 
            Availability status, 
            Bundle extras)
        {
            switch (status)
            {
                case Availability.Available:
                    OnProviderEnabled(provider);
                    break;

                case Availability.OutOfService:
                    OnProviderDisabled(provider);
                    break;
                case Availability.TemporarilyUnavailable:
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}