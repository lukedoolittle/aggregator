using System;
using Material;
using Material.Bluetooth;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using UIKit;

namespace Quantfabric.UI.Test.iOS
{
    using Twitter = Material.Infrastructure.ProtectedResources.Twitter;

    public partial class MainViewController : UIViewController
    {
        private AuthorizationInterface _browserType = 
            AuthorizationInterface.Embedded;
        private CallbackType _callbackType = 
            CallbackType.Localhost;

        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            BrowserToggle.TouchUpInside += (sender, args) =>
            {
                _browserType = BrowserToggle.On
                    ? AuthorizationInterface.Embedded
                    : AuthorizationInterface.Dedicated;
                _callbackType = BrowserToggle.On
                    ? CallbackType.Localhost
                    : CallbackType.Protocol;
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

                WriteOAuth1CredentialsToTextView(token);
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

                WriteOAuth1CredentialsToTextView(token);
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

                WriteOAuth1CredentialsToTextView(token);
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
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
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
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
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
                        .AddScope<GoogleGmail>()
                        .AddScope<GoogleProfile>()
                        .GetCredentialsAsync()
                        .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
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
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
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

                WriteOAuth2CredentialsToTextView(token);
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

                WriteOAuth2CredentialsToTextView(token);
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

                WriteOAuth2CredentialsToTextView(token);
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

                WriteOAuth2CredentialsToTextView(token);
            };

            InstagramAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Instagram>();
                var clientSecret = settings.GetClientSecret<Instagram>();
                var redirectUri = settings.GetRedirectUri<Instagram>();

                var token = await new OAuth2App<Instagram>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<InstagramLikes>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
            };

            PinterestAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Pinterest>();
                var clientSecret = settings.GetClientSecret<Pinterest>();
                var redirectUri = settings.GetRedirectUri<Pinterest>();

                var token = await new OAuth2App<Pinterest>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<PinterestLikes>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
            };

            TwentyThreeAndMeAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<TwentyThreeAndMe>();
                var clientSecret = settings.GetClientSecret<TwentyThreeAndMe>();
                var redirectUri = settings.GetRedirectUri<TwentyThreeAndMe>();

                var token = await new OAuth2App<TwentyThreeAndMe>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<TwentyThreeAndMeUser>()
                    .AddScope<TwentyThreeAndMeGenome>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
            };

            TumblrAuth.TouchUpInside += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var consumerKey = settings.GetConsumerKey<Tumblr>();
                var consumerSecret = settings.GetConsumerSecret<Tumblr>();
                var redirectUri = settings.GetRedirectUri<Tumblr>();

                var token = await new OAuth1App<Tumblr>(
                        consumerKey,
                        consumerSecret,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteOAuth1CredentialsToTextView(token);
            };

            AmazonAuth.TouchUpInside += async (sender, e) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Amazon>();
                var clientSecret = settings.GetClientSecret<Amazon>();
                var redirectUri = settings.GetRedirectUri<Amazon>();

                var token = await new OAuth2App<Amazon>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<AmazonProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteOAuth2CredentialsToTextView(token);
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

        private void WriteOAuth1CredentialsToTextView(OAuth1Credentials credentials)
        {
            if (credentials == null)
            {
                WriteResultToTextView("OAUTH1 WORKFLOW CANCELLED");
            }
            else
            {
                WriteResultToTextView("OAuth Secret: " + credentials.OAuthSecret + "\nOAuth Token: " + credentials.OAuthToken);
            }
        }

        private void WriteOAuth2CredentialsToTextView(OAuth2Credentials credentials)
        {
            if (credentials == null)
            {
                WriteResultToTextView("OAUTH2 WORKFLOW CANCELLED");
            }
            else
            {
                WriteResultToTextView("Access Token:" + credentials.AccessToken);
            }
        }

        private void WriteResultToTextView(string text)
        {
            InvokeOnMainThread(() => ResultsTextView.Text = text);
        }
    }
}