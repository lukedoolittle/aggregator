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

            FindViewById<Button>(Resource.Id.facebookAuth).Click += (sender, args) => 
            {
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

            FindViewById<Button>(Resource.Id.fatsecretAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.fitbitAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.foursquareAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.googleAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.linkedinAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.rescuetimeAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.spotifyAuth).Click += (sender, args) =>
            {
            };

            FindViewById<Button>(Resource.Id.runkeeperAuth).Click += (sender, args) =>
            {
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

