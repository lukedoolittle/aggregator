using System;
using Application.Configuration;
using Material;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using UIKit;

namespace Quantfabric.UI.Test.iOS
{
    using Twitter = Material.Infrastructure.ProtectedResources.Twitter;

    public partial class MainViewController : UIViewController
    {
        private AuthenticationInterfaceEnum _browserType = 
            AuthenticationInterfaceEnum.Embedded;
        private CallbackTypeEnum _callbackType = 
            CallbackTypeEnum.Localhost;

        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var settings = new CredentialApplicationSettings();

            BrowserToggle.TouchUpInside += (sender, args) =>
            {
                _browserType = BrowserToggle.On
                    ? AuthenticationInterfaceEnum.Embedded
                    : AuthenticationInterfaceEnum.Dedicated;
                _callbackType = BrowserToggle.On
                    ? CallbackTypeEnum.Localhost
                    : CallbackTypeEnum.Protocol;
            };

            FacebookAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Facebook, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            TwitterAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Twitter, OAuth1Credentials>(_callbackType);

                var token = await new OAuth1App<Twitter>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("OAuth Secret: " + token.OAuthSecret + "\nOAuth Token: " + token.OAuthToken);
            };
            FatsecretAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>(_callbackType);

                var token = await new OAuth1App<Fatsecret>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("OAuth Secret: " + token.OAuthSecret + "\nOAuth Token: " + token.OAuthToken);
            };
            WithingsAuth.TouchUpInside += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Withings, OAuth1Credentials>(_callbackType);

                var token = await new OAuth1App<Withings>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("OAuth Secret: " + token.OAuthSecret + "\nOAuth Token: " + token.OAuthToken);
            };
            SpotifyAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Spotify, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Spotify>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            GoogleAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Google, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Google>(
                            credentials.ClientId,
                            credentials.ClientSecret,
                            credentials.CallbackUrl,
                            browserType: _browserType)
                        .AddScope<GoogleGmailMetadata>()
                        .GetCredentialsAsync()
                        .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            FitbitAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fitbit, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Fitbit>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            RunkeeperAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Runkeeper, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Runkeeper>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                        .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            FoursquareAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Foursquare, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Foursquare>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            RescuetimeAuth.TouchUpInside += async (sender, e) =>
            {

                var credentials = settings
                    .GetClientCredentials<Rescuetime, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<Rescuetime>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            LinkedinAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<LinkedIn, OAuth2Credentials>(_callbackType);

                var token = await new OAuth2App<LinkedIn>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
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