using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using Material.Framework;

namespace Material.Permissions
{
    public static class DeviceAuthorizationFacadeExtensions
    {
        public static Task<bool> AuthorizeBluetooth(
            this DeviceAuthorizationFacade instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return instance.Authorize(
                new List<string>
                {
                    Manifest.Permission.Bluetooth,
                    Manifest.Permission.BluetoothPrivileged
                },
                0,
                Platform.Current.Context);
        }

        public static Task<bool> AuthorizeSMS(
            this DeviceAuthorizationFacade instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return instance.Authorize(
                new List<string>
                {
                    Manifest.Permission.ReadSms,
                    Manifest.Permission.ReadContacts
                },
                1,
                Platform.Current.Context);
        }

        public static Task<bool> AuthorizeGPS(
            this DeviceAuthorizationFacade instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return instance.Authorize(
                new List<string>
                {
                    Manifest.Permission.AccessFineLocation,
                    Manifest.Permission.AccessCoarseLocation
                },
                2,
                Platform.Current.Context);
        }
    }
}