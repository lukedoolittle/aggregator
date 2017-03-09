using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Material.OAuth.Workflow;
using Quantfabric.Test.Helpers;
using Quantfabric.UI.Test.UWP.Annotations;

namespace Quantfabric.UI.Test.UWP
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _result;

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public TestViewModel()
        {
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public sealed partial class MainPage : Page
    {
        private AuthorizationInterface _browserType =
            AuthorizationInterface.NotSpecified;
        private CallbackType _callbackType =
            CallbackType.NotSpecified;

        public MainPage()
        {
            this.InitializeComponent();
        }

        //FOR EASE OF COPY AND PASTE INTO WIKI

        private async void OnFacebookEmailClick(object sender, RoutedEventArgs e)
        {
            OAuth2Credentials credentials = await new OAuth2App<Facebook>(
                    "YOUR CLIENT ID",
                    "HTTP://YOURCALLBACKURI")
                .AddScope<FacebookUser>()
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            FacebookUserResponse user = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            string email = user.Email;
        }

        private async void OnGoogleEmailClick(object sender, RoutedEventArgs e)
        {
            OAuth2Credentials credentials = await new OAuth2App<Google>(
                    "YOUR CLIENT ID",
                    "HTTP://YOURCALLBACKURI")
                .AddScope<GoogleProfile>()
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            GoogleProfileResponse profile = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<GoogleProfile, GoogleProfileResponse>()
                .ConfigureAwait(false);

            string email = profile.Emails.First().Value;
        }

        private async void OnAmazonMobileClick(object sender, RoutedEventArgs e)
        {
            JsonWebToken credentialToken = "YOUR API KEY HERE".ToWebToken();

            OAuth2Credentials credentials = await new OAuth2App<Amazon>(
                    credentialToken.Claims.ClientId,
                    credentialToken.Claims.GetAmazonCallbackUri().ToCorrectedString())
                .AddScope<AmazonProfile>()
                .GetCredentialsAsync()
                .ConfigureAwait(false);
        }

        //END WIKI

        private async void OnFacebookClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
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
        }

        private async void OnTwitterClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnGoogleClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Google>();
            var redirectUri = settings.GetRedirectUri<Google>();

            var token = await new OAuth2App<Google>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<GoogleGmailMetadata>()
                    .AddScope<GoogleGmail>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteCredentials(token);
        }

        private async void OnFitbitClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Fitbit>();
            var redirectUri = settings.GetRedirectUri<Fitbit>();

            var token = await new OAuth2App<Fitbit>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteCredentials(token);
        }

        private async void OnPinterestClick(object sender, RoutedEventArgs e)
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

            WriteCredentials(token);
        }

        private async void OnRunkeeperClick(object sender, RoutedEventArgs e)
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

            WriteCredentials(token);
        }

        private async void OnInstagramClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
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
        }

        private async void OnTwentyThreeAndMeClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<TwentyThreeAndMe>();
            var clientSecret = settings.GetClientSecret<TwentyThreeAndMe>();
            var redirectUri = settings.GetRedirectUri<TwentyThreeAndMe>();

            var token = await new OAuth2App<TwentyThreeAndMe>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<TwentyThreeAndMeGenome>()
                    .AddScope<TwentyThreeAndMeUser>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            WriteCredentials(token);
        }

        private async void OnWithingsClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnTumblrClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnFatsecretClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnFoursquareClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Foursquare>();
            var redirectUri = settings.GetRedirectUri<Foursquare>();

            var token = await new OAuth2App<Foursquare>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FoursquareCheckin>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteCredentials(token);
        }

        private async void OnSpotifyClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnLinkedInClick(object sender, RoutedEventArgs e)
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

            WriteCredentials(token);
        }

        private async void OnRescuetimeClick(object sender, RoutedEventArgs e)
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

            WriteCredentials(token);
        }

        private async void OnAmazonClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnGoogleOpenIdClick(object sender, RoutedEventArgs e)
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
        }

        private async void OnYahooOpenIdClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Yahoo>();
            var redirectUri = "http://quantfabric.com/oauth/yah";

            var token = await new OpenIdApp<Yahoo>(
                        clientId,
                        redirectUri,
                        browserType: _browserType)
                    .GetWebTokenAsync()
                    .ConfigureAwait(false);

            WriteCredentials(token);
        }

        private void WriteCredentials(JsonWebToken credentials)
        {
            WriteToTextbox(credentials != null
                ? $"Json Web Token: {credentials.Signature}"
                : "OPENID OPERATION CANCELLED");
        }

        private void WriteCredentials(OAuth1Credentials credentials)
        {
            WriteToTextbox(credentials != null
                ? $"OAuthToken:{credentials.OAuthToken}, OAuthSecret:{credentials.OAuthSecret}"
                : "OAUTH1 OPERATION CANCELLED");
        }

        private void WriteCredentials(OAuth2Credentials credentials)
        {
            WriteToTextbox(credentials != null
                ? $"AccessToken:{credentials.AccessToken}"
                : "OAUTH2 OPERATION CANCELLED");
        }

        private void WriteToTextbox(string text)
        {
            Platform.Current.RunOnMainThread(() =>
            {
                ((TestViewModel) ((MainPage) Platform.Current.Context.Content).DataContext).Result = text;
            });
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Tag.ToString())
            {
                case "Unspecified":
                    _browserType = AuthorizationInterface.NotSpecified;
                    _callbackType = CallbackType.NotSpecified;
                    break;
                case "Embedded":
                    _browserType = AuthorizationInterface.Embedded;
                    _callbackType = CallbackType.Localhost;
                    break;
                case "Dedicated":
                    _browserType = AuthorizationInterface.Dedicated;
                    _callbackType = CallbackType.Protocol;
                    break;
            }
        }
    }
}
