using System;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Configuration;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Services;
using Aggregator.Task;
using Aggregator.Task.Authentication;
using Aggregator.View.BluetoothAuthorization;
using UIKit;

namespace Aggregator.UI.Test.iOS
{
    using Twitter = Aggregator.Infrastructure.Services.Twitter;

    public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : 
            base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            var factory = ServiceLocator.Current.GetInstance<iOSAuthenticationTaskFactory>();
		    var manager = ServiceLocator.Current.GetInstance<IBluetoothManager>();

            FacebookAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Facebook, OAuth2Credentials>(factory);
            };
            TwitterAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Twitter, OAuth1Credentials>(factory);
            };
            FatsecretAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Fatsecret, OAuth1Credentials>(factory);
            };
            SpotifyAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Spotify, OAuth2Credentials>(factory);
            };
            GoogleAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Google, OAuth2Credentials>(factory);
            };
            FitbitAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Fitbit, OAuth2Credentials>(factory);
            };
            RunkeeperAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Runkeeper, OAuth2Credentials>(factory);
            };
            FoursquareAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Foursquare, OAuth2Credentials>(factory);
            };
            RescuetimeAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Rescuetime, OAuth2Credentials>(factory);
            };
            LinkedinAuth.TouchUpInside += (sender, e) =>
            {
                FetchToken<Linkedin, OAuth2Credentials>(factory);
            };
		    MioAlphaAuth.TouchUpInside += async (sender, args) =>
		    {
                var aggregateId = Guid.NewGuid();
                var version = 0;

                var task = factory.GenerateTask<Mioalpha>(aggregateId, version, this)
                    as BluetoothAuthenticationTask<Mioalpha>;

                var address = await task.GetCredentials();
            };
		}

        private void FetchToken<TService, TCredentials>(iOSAuthenticationTaskFactory factory)
            where TService : Domain.Write.Service, new()
            where TCredentials : TokenCredentials
        {
            var aggregateId = Guid.NewGuid();
            var version = 0;

            var task =
                factory.GenerateTask<TService>(aggregateId, version, this) as
                    AuthenticationTaskBase<TCredentials, TService>;

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

