using System;
using Aggregator.Infrastructure;
using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Task;
using Aggregator.Task.Authentication;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Static;
using Fitbit = Material.Infrastructure.ProtectedResources.Fitbit;
using Google = Material.Infrastructure.ProtectedResources.Google;

namespace Aggregator.UI.Test.Android
{
    [Activity(Label = "Aggregator.UI.Test.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            await AggregatorService.Initializing;

            var factory = ServiceLocator.Current.GetInstance<AuthenticationTaskFactory>();

            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.facebookAuth).Click += (sender, args) => 
            {
                FetchToken<Facebook, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.twitterAuth).Click += (sender, args) =>
            {
                FetchToken<Twitter, OAuth1Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.fatsecretAuth).Click += (sender, args) =>
            {
                FetchToken<Fatsecret, OAuth1Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.fitbitAuth).Click += (sender, args) =>
            {
                FetchToken<Fitbit, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.foursquareAuth).Click += (sender, args) =>
            {
                FetchToken<Foursquare, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.googleAuth).Click += (sender, args) =>
            {
                FetchToken<Google, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.linkedinAuth).Click += (sender, args) =>
            {
                FetchToken<Linkedin, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.rescuetimeAuth).Click += (sender, args) =>
            {
                FetchToken<Rescuetime, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.spotifyAuth).Click += (sender, args) =>
            {
                FetchToken<Spotify, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.runkeeperAuth).Click += (sender, args) =>
            {
                FetchToken<Runkeeper, OAuth2Credentials>(factory);
            };

            FindViewById<Button>(Resource.Id.mioalphaAuth).Click += async (sender, args) =>
            {
                var task = factory.GenerateBluetoothTask<Mioalpha>(this);

                var address = await task.GetCredentials();
            };
        }


        private void FetchToken<TService, TCredentials>(AuthenticationTaskFactory factory)
            where TService : ResourceProvider, new()
            where TCredentials : TokenCredentials
        {
            var aggregateId = Guid.NewGuid();
            var version = 0;

            var task =
                factory.GenerateTask<TService>(aggregateId, version, this) as
                    OAuthAuthenticationTask<TCredentials, TService>;

            task.GetAccessTokenCredentials().ContinueWith(t =>
            {
                var credentials = t.Result;
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
            });
        }

        private void WriteResultToTextView(string result)
        {
            var resultView = FindViewById<TextView>(Resource.Id.resultView);

            RunOnUiThread(() => resultView.Text = result);
        }
    }
}

