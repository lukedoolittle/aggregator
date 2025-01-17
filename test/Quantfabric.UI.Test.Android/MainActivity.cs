﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using Material;
using Material.Application;
using Material.Bluetooth;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Domain.Requests;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;
using Material.Framework;
using Material.Framework.Enums;

namespace Quantfabric.UI.Test
{
    [Activity(Name = "quantfabric.ui.test.MainActivity")]
    public class MainActivity : Activity
    {
        private AuthorizationInterface _browserType = 
            AuthorizationInterface.NotSpecified;
        private CallbackType _callbackType =
            CallbackType.NotSpecified;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var toggleButton = FindViewById<RadioGroup>(Resource.Id.browserTypeRadioGroup);
            toggleButton.CheckedChange += (sender, args) =>
            {
                switch (toggleButton.CheckedRadioButtonId)
                {
                    case Resource.Id.unspecifiedRadioButton:
                        _browserType = AuthorizationInterface.NotSpecified;
                        _callbackType = CallbackType.NotSpecified;
                        break;
                    case Resource.Id.embeddedRadioButton:
                        _browserType = AuthorizationInterface.Embedded;
                        _callbackType = CallbackType.Localhost;
                        break;
                    case Resource.Id.secureRadioButton:
                        _browserType = AuthorizationInterface.SecureEmbedded;
                        _callbackType = CallbackType.Protocol;
                        break;
                    case Resource.Id.dedicatedRadioButton:
                        _browserType = AuthorizationInterface.Dedicated;
                        _callbackType = CallbackType.Protocol;
                        break;
                }
            };

            FindViewById<Button>(Resource.Id.twitterAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.fatsecretAuth).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.withings).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.facebookAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
                var clientId = settings.GetClientId<Facebook>();
                var redirectUri = settings.GetRedirectUri<Facebook>();

                var token = await new OAuth2App<Facebook>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.fitbitAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Fitbit>();
                var redirectUri = settings.GetRedirectUri<Fitbit>();

                var token = await new OAuth2App<Fitbit>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.foursquareAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Foursquare>();
                var redirectUri = settings.GetRedirectUri<Foursquare>();

                var token = await new OAuth2App<Foursquare>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.googleAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Google>();
                var redirectUri = settings.GetRedirectUri<Google>();

                var token = await new OAuth2App<Google>(
                            clientId,
                            redirectUri,
                            browserType: _browserType)
                        .AddScope<GoogleGmailMetadata>()
                        .AddScope<GoogleProfile>()
                        .GetCredentialsAsync()
                        .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.linkedinAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.rescuetimeAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.spotifyAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Spotify>();
                var redirectUri = settings.GetRedirectUri<Spotify>();

                var token = await new OAuth2App<Spotify>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.runkeeperAuth).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.pinterest).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
                var clientId = settings.GetClientId<Pinterest>();
                var clientSecret = settings.GetClientSecret<Pinterest>();
                var redirectUri = settings.GetRedirectUri<Pinterest>();

                var token = await new OAuth2App<Pinterest>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                        .AddScope<PinterestPins>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.instagram).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
                var clientId = settings.GetClientId<Instagram>();
                var redirectUri = settings.GetRedirectUri<Instagram>();

                var token = await new OAuth2App<Instagram>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                        .AddScope<InstagramLikes>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.tumblr).Click += async (sender, args) =>
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.twentythreeandme).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(CallbackType.Localhost);
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

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.amazon).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Amazon>();
                var redirectUri = settings.GetRedirectUri<Amazon>();

                var token = await new OAuth2App<Amazon>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<AmazonProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.GoogleOpenId).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Google>();
                var redirectUri = settings.GetRedirectUri<Google>();

                var token = await new OpenIdApp<Google>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .GetWebTokenAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.YahooOpenId).Click += async (sender, args) =>
            {
                var settings = new AppCredentialRepository(_callbackType);
                var clientId = settings.GetClientId<Yahoo>();
                var redirectUri = settings.GetRedirectUri<Yahoo>();

                var token = await new OpenIdApp<Yahoo>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .GetWebTokenAsync()
                    .ConfigureAwait(false);

                WriteCredentials(token);
            };

            FindViewById<Button>(Resource.Id.gps).Click += async (sender, args) =>
            {
                var result = await new GPSRequester()
                    .MakeGPSRequestAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView(
                    $"Latitude: {result.Latitude}, Longitude: {result.Longitude}, Speed: {result.Speed}");
            };

            FindViewById<Button>(Resource.Id.sms).Click += async (sender, args) =>
            {
                var filterDate = new DateTime(2016, 7, 21, 0, 0, 0);
                var results = await new SMSRequester()
                    .MakeSMSRequestAsync(filterDate)
                    .ConfigureAwait(false);

                var resultsString = string.Empty;
                foreach (var result in results)
                {
                    resultsString +=
                        $"Address: {result.Address}, Header: {result.Subject}, Body: {result.Body}\n";
                }
                WriteResultToTextView(resultsString);
            };

            FindViewById<Button>(Resource.Id.mioalphaAuth).Click += async (sender, args) =>
            {
                var credentials = await new BluetoothApp<Mioalpha>()
                    .GetBluetoothCredentialsAsync()
                    .ConfigureAwait(false);

                WriteResultToTextView("Device Address: " + credentials.DeviceAddress);

                var result = await new BluetoothRequester()
                    .MakeBluetoothRequestAsync<MioHeartRate>(credentials)
                    .ConfigureAwait(false);

                WriteResultToTextView("Heart rate: " + result.Reading);
            };

            ISubscriptionManager subscription = null;

            FindViewById<Button>(Resource.Id.mioalphaContinuousAuth).Click += async (sender, args) =>
            {
                if (subscription != null)
                {
                    subscription.Unsubscribe();
                    WriteResultToTextView("Unsubscribed!!!");
                    subscription = null;
                }
                else
                {
                    var credentials = await new BluetoothApp<Mioalpha>()
                        .GetBluetoothCredentialsAsync()
                        .ConfigureAwait(false);

                    WriteResultToTextView("Device Address: " + credentials.DeviceAddress);

                    subscription = await new BluetoothRequester()
                        .SubscribeToBluetoothRequest<MioHeartRate>(
                            credentials,
                            response =>
                            {
                                WriteResultToTextView("Heart rate: " + response.Reading);
                            })
                        .ConfigureAwait(false);

                    if (subscription == null)
                    {
                        WriteResultToTextView("Couldn't subscribe!!!");
                    }
                }
            };
        }

        private void WriteCredentials(object credentials)
        {
            if (credentials == null)
            {
                WriteResultToTextView("WORKFLOW WAS CANCELLED");
            }
            else if (credentials is OAuth1Credentials)
            {
                var token = credentials as OAuth1Credentials;
                WriteResultToTextView($"OAuthToken: {token.OAuthToken}\nOAuthSecret: {token.OAuthSecret}");
            }
            else if (credentials is OAuth2Credentials)
            {
                var token = credentials as OAuth2Credentials;
                WriteResultToTextView($"AccessToken: {token.AccessToken}");
            }
            else if (credentials is JsonWebToken)
            {
                var token = credentials as JsonWebToken;
                WriteResultToTextView($"JWT: {token.Signature}");
            }
            else
            {
                WriteResultToTextView("Error in credentials type :" + credentials.GetType().Name);
            }
        }

        private void WriteResultToTextView(string result)
        {
            RunOnUiThread(async () =>
            {
                TextView resultView = null;

                while (resultView == null)
                {
                    await Task.Delay(100);

                    resultView = Platform
                        .Current
                        .Context
                        .FindViewById<TextView>(Resource.Id.resultView);
                }
                resultView.Text = result;
            });
        }
    }
}

