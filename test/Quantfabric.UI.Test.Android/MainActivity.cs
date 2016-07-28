using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Application.Configuration;
using Material;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;

namespace Quantfabric.UI.Test.Android
{
    [Activity(Label = "Quantfabric.UI.Test.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var settings = new CredentialApplicationSettings();

            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.facebookAuth).Click += async (sender, args) => 
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.twitterAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Twitter, OAuth1Credentials>();

                var token = await new OAuth1AppFacade<Twitter>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.fatsecretAuth).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>();

                var token = await new OAuth1AppFacade<Fatsecret>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };
            FindViewById<Button>(Resource.Id.withings).Click += async (sender, args) =>
            {
                var credentials = settings
                    .GetClientCredentials<Withings, OAuth1Credentials>();

                var token = await new OAuth1AppFacade<Withings>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };
            FindViewById<Button>(Resource.Id.fitbitAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.foursquareAuth).Click += async (sender, args) =>
            {

                var credentials = settings
                    .GetClientCredentials<Foursquare, OAuth2Credentials>();

                var token = await new OAuth2AppFacade<Foursquare>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.googleAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.linkedinAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.rescuetimeAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.spotifyAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.runkeeperAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.mioalphaAuth).Click += async (sender, args) =>
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

            FindViewById<Button>(Resource.Id.gps).Click += async (sender, args) =>
            {
                var result = await new GPSRequester()
                    .MakeGPSRequest()
                    .ConfigureAwait(false);

                WriteResultToTextView($"Latitude: {result.Latitude}, Longitude: {result.Longitude}, Speed: {result.Speed}");
            };

            FindViewById<Button>(Resource.Id.sms).Click += async (sender, args) =>
            {
                var result = await new SMSRequester()
                    .MakeSMSRequest()
                    .ConfigureAwait(false);

                var firstResult = result.First();
                WriteResultToTextView($"Address: {firstResult.Address}, Creator: {firstResult.Creator} Header: {firstResult.Subject}, Body: {firstResult.Body}");
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

