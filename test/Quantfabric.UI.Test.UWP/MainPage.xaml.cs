using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Application.Configuration;
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

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnFacebookClick(object sender, RoutedEventArgs e)
        {
            var credentials = _settings
                .GetClientCredentials<Facebook, OAuth2Credentials>();

            var token = await new OAuth2App<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteToTextbox($"AccessToken:{token.AccessToken}");
        }

        private async void OnTwitterClick(object sender, RoutedEventArgs e)
        {
            var credentials = _settings
                .GetClientCredentials<Twitter, OAuth1Credentials>();

            var token = await new OAuth1App<Twitter>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteToTextbox($"OAuthToken:{token.OAuthToken}, OAuthSecret:{token.OAuthSecret}");
        }

        private void WriteToTextbox(string text)
        {
            Platform.RunOnMainThread(() =>
            {
                ResultTextBlock.Text = text;
                ResultTextBlock.UpdateLayout();
            });
        }
    }
}
