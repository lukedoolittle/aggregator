using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Application.Configuration;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;

namespace Quantfabric.UI.Test.UWP
{
    public sealed partial class MainPage : Page
    {
        private readonly CredentialApplicationSettings _settings =
            new CredentialApplicationSettings();

        private AuthenticationInterfaceEnum _browserType =
            AuthenticationInterfaceEnum.Embedded;
        private CallbackTypeEnum _callbackType =
            CallbackTypeEnum.Localhost;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnFacebookClick(object sender, RoutedEventArgs e)
        {
            var credentials = _settings
                .GetClientCredentials<Facebook, OAuth2Credentials>(_callbackType);

            var token = await new OAuth2App<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteToTextbox($"AccessToken:{token.AccessToken}");
        }

        private async void OnTwitterClick(object sender, RoutedEventArgs e)
        {
            var credentials = _settings
                .GetClientCredentials<Twitter, OAuth1Credentials>(_callbackType);

            var token = await new OAuth1App<Twitter>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteToTextbox($"OAuthToken:{token.OAuthToken}, OAuthSecret:{token.OAuthSecret}");
        }

        private async void OnGoogleClick(object sender, RoutedEventArgs e)
        {
            var credentials = _settings
                .GetClientCredentials<Google, OAuth2Credentials>(_callbackType);

            var token = await new OAuth2App<Google>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl,
                        browserType: _browserType)
                    .AddScope<GoogleGmailMetadata>()
                    .AddScope<GoogleGmail>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteToTextbox($"AccessToken:{token.AccessToken}");
        }

        private void WriteToTextbox(string text)
        {
            Platform.Current.RunOnMainThread(() =>
            {
                ResultTextBlock.Text = text;
            });
        }

        private void BrowserTypeToggled(object sender, RoutedEventArgs e)
        {
            _browserType = authTypeToggleSwitch.IsOn
                ? AuthenticationInterfaceEnum.Dedicated
                : AuthenticationInterfaceEnum.Embedded;
            _callbackType = authTypeToggleSwitch.IsOn
                ? CallbackTypeEnum.Protocol 
                : CallbackTypeEnum.Localhost;
        }
    }
}
