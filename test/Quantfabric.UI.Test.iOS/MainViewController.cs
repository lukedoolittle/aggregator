using Foundation;
using System;
using Aggregator.Framework.Contracts;
using Aggregator.Task;
using Aggregator.Task.Authentication;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Static;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace Aggregator.UI.Test.iOS
{
    public partial class MainViewController : UIViewController
    {
        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var factory = ServiceLocator.Current.GetInstance<AuthenticationTaskFactory>();
            var manager = ServiceLocator.Current.GetInstance<IBluetoothManager>();

            FacebookAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Facebook, OAuth2Credentials>(factory);
            };
            TwitterAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Material.Infrastructure.ProtectedResources.Twitter, OAuth1Credentials>(factory);
            };
            //FatsecretAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Fatsecret, OAuth1Credentials>(factory);
            //};
            //SpotifyAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Spotify, OAuth2Credentials>(factory);
            //};
            //GoogleAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Google, OAuth2Credentials>(factory);
            //};
            //FitbitAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Fitbit, OAuth2Credentials>(factory);
            //};
            //RunkeeperAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Runkeeper, OAuth2Credentials>(factory);
            //};
            //FoursquareAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Foursquare, OAuth2Credentials>(factory);
            //};
            //RescuetimeAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Rescuetime, OAuth2Credentials>(factory);
            //};
            //LinkedinAuth.TouchUpInside += (sender, e) =>
            //{
            //    FetchToken<Linkedin, OAuth2Credentials>(factory);
            //};
            MioAuth.TouchUpInside += async (sender, args) =>
            {
                var aggregateId = Guid.NewGuid();
                var version = 0;

                var task = factory.GenerateTask<Mioalpha>(aggregateId, version, this)
                       as BluetoothAuthenticationTask<Mioalpha>;

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

        private void WriteResultToTextView(string text)
        {
            InvokeOnMainThread(() => ResultsTextView.Text = text);
        }
    }
}