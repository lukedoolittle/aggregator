using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Material;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;

namespace Quantfabric.UI.Test
{
    [Activity(Label = "Quantfabric.UI.Test.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private AuthenticationInterfaceEnum _browserType = 
            AuthenticationInterfaceEnum.Embedded;
        private CallbackTypeEnum _callbackType =
            CallbackTypeEnum.Localhost;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var toggleButton = FindViewById<ToggleButton>(Resource.Id.browserToggleButton);
            toggleButton.Click += (o, e) =>
            {
                _browserType = toggleButton.Checked
                    ? AuthenticationInterfaceEnum.Dedicated
                    : AuthenticationInterfaceEnum.Embedded;
                _callbackType = toggleButton.Checked
                    ? CallbackTypeEnum.Protocol
                    : CallbackTypeEnum.Localhost;
            };

            FindViewById<Button>(Resource.Id.twitterAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var consumerKey = settings.GetConsumerKey<Twitter>();
                var consumerSecret = settings.GetConsumerSecret<Twitter>();
                var redirectUri = settings.GetRedirectUri<Twitter>();

                var token = await new OAuth1App<Twitter>(
                        consumerKey,
                        consumerSecret,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.fatsecretAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var consumerKey = settings.GetConsumerKey<Fatsecret>();
                var consumerSecret = settings.GetConsumerSecret<Fatsecret>();
                var redirectUri = settings.GetRedirectUri<Fatsecret>();

                var token = await new OAuth1App<Fatsecret>(
                        consumerKey,
                        consumerSecret,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.withings).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var consumerKey = settings.GetConsumerKey<Withings>();
                var consumerSecret = settings.GetConsumerSecret<Withings>();
                var redirectUri = settings.GetRedirectUri<Withings>();

                var token = await new OAuth1App<Withings>(
                        consumerKey,
                        consumerSecret,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.facebookAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Facebook>();
                var clientSecret = settings.GetClientSecret<Facebook>();
                var redirectUri = settings.GetRedirectUri<Facebook>();

                var token = await new OAuth2App<Facebook>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.fitbitAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Fitbit>();
                var clientSecret = settings.GetClientSecret<Fitbit>();
                var redirectUri = settings.GetRedirectUri<Fitbit>();

                var token = await new OAuth2App<Fitbit>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.foursquareAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Foursquare>();
                var clientSecret = settings.GetClientSecret<Foursquare>();
                var redirectUri = settings.GetRedirectUri<Foursquare>();

                var token = await new OAuth2App<Foursquare>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.googleAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Google>();
                var clientSecret = settings.GetClientSecret<Google>();
                var redirectUri = settings.GetRedirectUri<Google>();

                var token = await new OAuth2App<Google>(
                        clientId,
                        clientSecret,
                        redirectUri,
                            browserType: _browserType)
                        .AddScope<GoogleGmailMetadata>()
                        .GetCredentialsAsync()
                        .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.linkedinAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<LinkedIn>();
                var clientSecret = settings.GetClientSecret<LinkedIn>();
                var redirectUri = settings.GetRedirectUri<LinkedIn>();

                var token = await new OAuth2App<LinkedIn>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<LinkedinPersonal>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.rescuetimeAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Rescuetime>();
                var clientSecret = settings.GetClientSecret<Rescuetime>();
                var redirectUri = settings.GetRedirectUri<Rescuetime>();

                var token = await new OAuth2App<Rescuetime>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.spotifyAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Spotify>();
                var clientSecret = settings.GetClientSecret<Spotify>();
                var redirectUri = settings.GetRedirectUri<Spotify>();

                var token = await new OAuth2App<Spotify>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.runkeeperAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Runkeeper>();
                var clientSecret = settings.GetClientSecret<Runkeeper>();
                var redirectUri = settings.GetRedirectUri<Runkeeper>();

                var token = await new OAuth2App<Runkeeper>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                        .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };



            FindViewById<Button>(Resource.Id.mioalphaAuth).Click += async (sender, args) =>
            {
                var credentials = await new BluetoothApp<Mioalpha>()
                    .GetBluetoothCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Device Address: " + credentials.DeviceAddress);

                var result = await new BluetoothRequester()
                    .MakeBluetoothRequestAsync<MioHeartRate>(credentials)
                    .ConfigureAwait(false);

                WriteResultToTextView("Heart rate: " + result.Reading);
            };

            FindViewById<Button>(Resource.Id.gps).Click += async (sender, args) =>
            {
                var result = await new GPSRequester()
                    .MakeGPSRequestAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView(
                    $"Latitude: {result.Latitude}, Longitude: {result.Longitude}, Speed: {result.Speed}");
            };

            FindViewById<Button>(Resource.Id.sms).Click += async (sender, args) =>
            {
                var filterDate = new DateTime(2016, 7, 21, 0, 0, 0);
                var results = await new SMSRequester()
                    .MakeSMSRequestAsync(filterDate)
                    .ConfigureAwait(false);

                var resultsString = string.Empty;
                foreach (var result in results)
                {
                    resultsString +=
                        $"Address: {result.Address}, Header: {result.Subject}, Body: {result.Body}\n";
                }
                WriteResultToTextView(resultsString);
            };
        }

        private void WriteCredentials(object credentials)
        {
            if (credentials is OAuth1Credentials)
            {
                var token = credentials as OAuth1Credentials;
                WriteResultToTextView($"OAuthToken: {token.OAuthToken}\nOAuthSecret: {token.OAuthSecret}");
            }
            else if (credentials is OAuth2Credentials)
            {
                var token = credentials as OAuth2Credentials;
                WriteResultToTextView($"AccessToken: {token.AccessToken}");
            }
            else
            {
                WriteResultToTextView("Error in credentials type :" + credentials.GetType().Name);
            }
        }

        private void WriteResultToTextView(string result)
        {
            var resultView = FindViewById<TextView>(Resource.Id.resultView);

            RunOnUiThread(() => resultView.Text = result);
        }
    }
}

