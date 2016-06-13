using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Aggregator.Configuration
{
    public static class UserSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        private const string IdKey = "myId";
        private static readonly Guid IdDefault = Guid.Empty;

        public static Guid UserId
        {
            get { return AppSettings.GetValueOrDefault(IdKey, IdDefault); }
            set { AppSettings.AddOrUpdateValue(IdKey, value); }
        }
    }
}
