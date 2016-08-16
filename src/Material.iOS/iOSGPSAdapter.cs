using System;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using Material.Contracts;
using Material.Infrastructure.Static;
using System.Timers;
using Material.Exceptions;
using UIKit;

namespace Material
{
    public class iOSGPSAdapter : IGPSAdapter
    {
        private readonly object _syncLock = new object();

        private readonly CLLocationManager _locationManager;
        private TaskCompletionSource<GPSResponse> _completionSource;
        private bool _isListening = false;
        private Timer _timer;
        private readonly int _timeoutInMs;
        private readonly float _desiredAccuracyInMeters;

        public iOSGPSAdapter(
            CLLocationManager locationManager,
            int gpsTimeoutInMs = 30000,
            float desiredAccuracyInMeters = 50)
        {
            _locationManager = locationManager;

            _locationManager.PausesLocationUpdatesAutomatically = false;
            _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
            _locationManager.ActivityType = CLActivityType.Other;
            _locationManager.DistanceFilter = CLLocationDistance.FilterNone;

            _locationManager.DisallowDeferredLocationUpdates();

            _timeoutInMs = gpsTimeoutInMs;
            _desiredAccuracyInMeters = desiredAccuracyInMeters;
        }

        public Task<GPSResponse> GetPositionAsync()
        {
            if (_isListening)
            {
                return _completionSource.Task;
            }

            lock (_syncLock)
            {
                if (CLLocationManager.LocationServicesEnabled)
                {
                    _isListening = true;
                    StartTimer();
                    StartLocationManager();
                    _completionSource =
                        new TaskCompletionSource<GPSResponse>();
                }
                else
                {
                    //fails silently!!!
                }

                return _completionSource.Task;
            }
        }

        private void StartTimer()
        {
            _timer = new Timer(_timeoutInMs);
            _timer.Elapsed += (sender, args) =>
            {
                Cleanup();
                throw new LocationException(
                    StringResources.GPSTimeoutConnectivityException);
            };
            _timer.Start();
        }

        private void StopTimer()
        {
            _timer.Stop();
            _timer = null;
        }

        private void StartLocationManager()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
                _locationManager.LocationsUpdated +=
                    LocationManagerOnLocationsUpdated;
            else
                _locationManager.UpdatedLocation +=
                    LocationManagerOnUpdatedLocation;

            _locationManager.Failed +=
                LocationManagerOnFailed;
            _locationManager.StartUpdatingLocation();
        }

        private void StopLocationManager()
        {
            _locationManager.StopUpdatingLocation();
            _locationManager.LocationsUpdated -=
                LocationManagerOnLocationsUpdated;
            _locationManager.UpdatedLocation -=
                LocationManagerOnUpdatedLocation;
            _locationManager.Failed -=
                LocationManagerOnFailed;
        }

        private void LocationManagerOnFailed(
            object sender, 
            NSErrorEventArgs nsErrorEventArgs)
        {
            throw new LocationException(
                StringResources.GPSDisabledConnectivityException);
        }

        private void LocationManagerOnUpdatedLocation(
            object sender,
            CLLocationUpdatedEventArgs clLocationUpdatedEventArgs)
        {
            OnLocationChanged(
                clLocationUpdatedEventArgs.NewLocation);
        }

        private void LocationManagerOnLocationsUpdated(
            object sender, 
            CLLocationsUpdatedEventArgs e)
        {
            var location = e.Locations[e.Locations.Length - 1];

            OnLocationChanged(location);
        }

        private void OnLocationChanged(CLLocation location)
        {
            lock (_syncLock)
            {
                if (_isListening &&
                    location.HorizontalAccuracy < _desiredAccuracyInMeters)
                {
                    var result = new GPSResponse
                    {
                        Altitude = location.Altitude,
                        Longitude = location.Coordinate.Longitude,
                        Latitude = location.Coordinate.Latitude,
                        Heading = location.Course,
                        Speed = location.Speed,
                        Timestamp = ToDateTime(location.Timestamp),
                        Accuracy = location.HorizontalAccuracy,
                        AltitudeAccuracy = location.VerticalAccuracy
                    };

                    _completionSource.SetResult(result);

                    Cleanup();
                }
            }
        }

        private void Cleanup()
        {
            StopTimer();
            StopLocationManager();
            _completionSource = null;
            _isListening = false;
        }

        private static DateTime ToDateTime(NSDate date)
        {
            var reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }
    }
}
