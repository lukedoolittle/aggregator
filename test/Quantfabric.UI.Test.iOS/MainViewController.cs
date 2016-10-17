using System;
using Material;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;
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

            BrowserToggle.TouchUpInside += (sender, args) =>
            {
                _browserType = BrowserToggle.On
                    ? AuthenticationInterfaceEnum.Embedded
                    : AuthenticationInterfaceEnum.Dedicated;
                _callbackType = BrowserToggle.On
                    ? CallbackTypeEnum.Localhost
                    : CallbackTypeEnum.Protocol;
            };

            TwitterAuth.TouchUpInside += async (sender, e) =>
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

                WriteResultToTextView("OAuth Secret: " + token.OAuthSecret + "\nOAuth Token: " + token.OAuthToken);
            };

            FatsecretAuth.TouchUpInside += async (sender, e) =>
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

                WriteResultToTextView("OAuth Secret: " + token.OAuthSecret + "\nOAuth Token: " + token.OAuthToken);
            };

            WithingsAuth.TouchUpInside += async (sender, args) =>
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

                WriteResultToTextView("OAuth Secret: " + token.OAuthSecret + "\nOAuth Token: " + token.OAuthToken);
            };

            FacebookAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Facebook>();
                var clientSecret = settings.GetClientSecret<Facebook>();
                var redirectUri = settings.GetRedirectUri<Facebook>();

                var token = await new OAuth2App<Facebook>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            SpotifyAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Spotify>();
                var clientSecret = settings.GetClientSecret<Spotify>();
                var redirectUri = settings.GetRedirectUri<Spotify>();

                var token = await new OAuth2App<Spotify>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            GoogleAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Google>();
                var clientSecret = settings.GetClientSecret<Google>();
                var redirectUri = settings.GetRedirectUri<Google>();

                var token = await new OAuth2App<Google>(
                            clientId,
                            redirectUri,
                            browserType: _browserType)
                        .AddScope<GoogleGmailMetadata>()
                        .GetCredentialsAsync(clientSecret)
                        .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            FitbitAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Fitbit>();
                var clientSecret = settings.GetClientSecret<Fitbit>();
                var redirectUri = settings.GetRedirectUri<Fitbit>();

                var token = await new OAuth2App<Fitbit>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            RunkeeperAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Runkeeper>();
                var clientSecret = settings.GetClientSecret<Runkeeper>();
                var redirectUri = settings.GetRedirectUri<Runkeeper>();

                var token = await new OAuth2App<Runkeeper>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                        .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            FoursquareAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Foursquare>();
                var clientSecret = settings.GetClientSecret<Foursquare>();
                var redirectUri = settings.GetRedirectUri<Foursquare>();

                var token = await new OAuth2App<Foursquare>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            RescuetimeAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Rescuetime>();
                var clientSecret = settings.GetClientSecret<Rescuetime>();
                var redirectUri = settings.GetRedirectUri<Rescuetime>();

                var token = await new OAuth2App<Rescuetime>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };

            LinkedinAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<LinkedIn>();
                var clientSecret = settings.GetClientSecret<LinkedIn>();
                var redirectUri = settings.GetRedirectUri<LinkedIn>();

                var token = await new OAuth2App<LinkedIn>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<LinkedinPersonal>()
                    .GetCredentialsAsync(clientSecret)
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