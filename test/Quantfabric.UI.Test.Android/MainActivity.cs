using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Application.Configuration;
using Foundations.Http;
using Material;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;

namespace Quantfabric.UI.Test
{
    [Activity(Label = "Quantfabric.UI.Test.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private AuthenticationInterfaceEnum _browserType = 
            AuthenticationInterfaceEnum.Embedded;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var settings = new CredentialApplicationSettings();

            SetContentView(Resource.Layout.Main);

            var toggleButton = FindViewById<ToggleButton>(Resource.Id.browserToggleButton);
            toggleButton.Click += (o, e) =>
            {
                _browserType = toggleButton.Checked
                    ? AuthenticationInterfaceEnum.Dedicated
                    : AuthenticationInterfaceEnum.Embedded;
            };

            FindViewById<Button>(Resource.Id.facebookAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Facebook, OAuth2Credentials>();

                var token = await new OAuth2App<Facebook>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.twitterAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Twitter, OAuth1Credentials>();

                var token = await new OAuth1App<Twitter>(
                    credentials.ConsumerKey,
                    credentials.ConsumerSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.fatsecretAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>();

                var token = await new OAuth1App<Fatsecret>(
                    credentials.ConsumerKey,
                    credentials.ConsumerSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };
            FindViewById<Button>(Resource.Id.withings).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Withings, OAuth1Credentials>();

                var token = await new OAuth1App<Withings>(
                    credentials.ConsumerKey,
                    credentials.ConsumerSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };
            FindViewById<Button>(Resource.Id.fitbitAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fitbit, OAuth2Credentials>();

                var token = await new OAuth2App<Fitbit>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.foursquareAuth).Click += async (sender, args) =>
            {

                var credentials = settings
                    .GetClientCredentials<Foursquare, OAuth2Credentials>();

                var token = await new OAuth2App<Foursquare>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.googleAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Google, OAuth2Credentials>();

                var token = await new OAuth2App<Google>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .AddScope<GoogleGmailMetadata>()
                    .AddScope<GoogleGmail>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.linkedinAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<LinkedIn, OAuth2Credentials>();

                var token = await new OAuth2App<LinkedIn>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .AddScope<LinkedinPersonal>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.rescuetimeAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Rescuetime, OAuth2Credentials>();

                var token = await new OAuth2App<Rescuetime>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.spotifyAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Spotify, OAuth2Credentials>();

                var token = await new OAuth2App<Spotify>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
                    browserType: _browserType)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.runkeeperAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Runkeeper, OAuth2Credentials>();

                var token = await new OAuth2App<Runkeeper>(
                    credentials.ClientId,
                    credentials.ClientSecret,
                    credentials.CallbackUrl,
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

