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

                var token = await new OAuth2AppFacade<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            TwitterAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Material.Infrastructure.ProtectedResources.Twitter, OAuth1Credentials>();

                var token = await new OAuth1AppFacade<Material.Infrastructure.ProtectedResources.Twitter>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.OAuthSecret);
            };
            FatsecretAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>();

                var token = await new OAuth1AppFacade<Fatsecret>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.OAuthSecret);
            };
            WithingsAuth.TouchUpInside += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Withings, OAuth1Credentials>();

                var token = await new OAuth1AppFacade<Withings>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.OAuthSecret);
            };
            SpotifyAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Spotify, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Spotify>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<SpotifySavedTrack>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView(token.AccessToken);
            };
            GoogleAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Google, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Google>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<GoogleGmailMetadata>()
                    .AddScope<GoogleGmail>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            FitbitAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fitbit, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Fitbit>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FitbitProfile>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            RunkeeperAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Runkeeper, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Runkeeper>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                        .AddScope<RunkeeperFitnessActivity>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            FoursquareAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<Foursquare, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Foursquare>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            RescuetimeAuth.TouchUpInside += async (sender, e) =>
            {

                var credentials = settings
                    .GetClientCredentials<Rescuetime, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Rescuetime>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            LinkedinAuth.TouchUpInside += async (sender, e) =>
            {
                var credentials = settings
                    .GetClientCredentials<LinkedIn, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<LinkedIn>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<LinkedinPersonal>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Access Token:" + token.AccessToken);
            };
            MioAuth.TouchUpInside += async (sender, args) =>
            {
                var auth = new BluetoothAuthorizationFacade<Mioalpha>();
                var credentials = await auth.GetBluetoothCredentials()
                    .ConfigureAwait(false);

                WriteResultToTextView("Device Address: " + credentials.DeviceAddress);

                var requester = new BluetoothRequester();
                var result = await requester
                    .MakeBluetoothRequest<MioHeartRate>(credentials)
                    .ConfigureAwait(false);

                WriteResultToTextView("Heart rate: " + result.Reading);
            };
            GPS.TouchUpInside += async (sender, args) =>
            {
                await new GPSAuthorizationFacade()
                    .AuthorizeContinuousGPSUsage()
                    .ConfigureAwait(false);
                var result = await new GPSRequester()
                    .MakeGPSRequest()
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