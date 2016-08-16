using System;
using Application.Configuration;
using Material;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using UIKit;

namespace Quantfabric.UI.Test.iOS
{
    public partial class MainViewController : UIViewController
    {
        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var settings = new CredentialApplicationSettings();

            FacebookAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Facebook, OAuth2Credentials>();

                var token = await new OAuth2App<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            TwitterAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Material.Infrastructure.ProtectedResources.Twitter, OAuth1Credentials>();

                var token = await new OAuth1App<Material.Infrastructure.ProtectedResources.Twitter>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.OAuthSecret);
            };
            FatsecretAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>();

                var token = await new OAuth1App<Fatsecret>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.OAuthSecret);
            };
            WithingsAuth.TouchUpInside += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Withings, OAuth1Credentials>();

                var token = await new OAuth1App<Withings>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.OAuthSecret);
            };
            SpotifyAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Spotify, OAuth2Credentials>();

                var token = await new OAuth2App<Spotify>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.AccessToken);
            };
            GoogleAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Google, OAuth2Credentials>();

                var token = await new OAuth2App<Google>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<GoogleGmailMetadata>()
                    .AddScope<GoogleGmail>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            FitbitAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fitbit, OAuth2Credentials>();

                var token = await new OAuth2App<Fitbit>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            RunkeeperAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Runkeeper, OAuth2Credentials>();

                var token = await new OAuth2App<Runkeeper>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                        .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            FoursquareAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Foursquare, OAuth2Credentials>();

                var token = await new OAuth2App<Foursquare>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            RescuetimeAuth.TouchUpInside += async (sender, e) =>
            {

                var credentials = settings
                    .GetClientCredentials<Rescuetime, OAuth2Credentials>();

                var token = await new OAuth2App<Rescuetime>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            LinkedinAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<LinkedIn, OAuth2Credentials>();

                var token = await new OAuth2App<LinkedIn>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<LinkedinPersonal>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            MioAuth.TouchUpInside += async (sender, args) =>
            {
                var auth = new BluetoothApp<Mioalpha>();
                var credentials = await auth.GetBluetoothCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Device Address: " + credentials.DeviceAddress);

                var requester = new BluetoothRequester();
                var result = await requester
                    .MakeBluetoothRequestAsync<MioHeartRate>(credentials)
                    .ConfigureAwait(false);

                WriteResultToTextView("Heart rate: " + result.Reading);
            };
            GPS.TouchUpInside += async (sender, args) =>
            {
                var result = await new GPSRequester()
                    .MakeGPSRequestAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView($"Latitude: {result.Latitude}, Longitude: {result.Longitude}, Speed: {result.Speed}");
            };
        }

        private void WriteResultToTextView(string text)
        {
            InvokeOnMainThread(() => ResultsTextView.Text = text);
        }
    }
}