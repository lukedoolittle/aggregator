using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
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
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Facebook>();
            var clientSecret = settings.GetClientSecret<Facebook>();
            var redirectUri = settings.GetRedirectUri<Facebook>();

            var token = await new OAuth2App<Facebook>(
                        clientId,
                        clientSecret,
                        redirectUri,
                        browserType: _browserType)
                    .AddScope<FacebookEvent>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            WriteToTextbox($"AccessToken:{token.AccessToken}");
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

            WriteToTextbox($"OAuthToken:{token.OAuthToken}, OAuthSecret:{token.OAuthSecret}");
        }

        private async void OnGoogleClick(object sender, RoutedEventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Google>();
            var clientSecret = settings.GetClientSecret<Google>();
            var redirectUri = settings.GetRedirectUri<Google>();

            var token = await new OAuth2App<Google>(
                        clientId,
                        clientSecret,
                        redirectUri,
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
                ((TestViewModel) ((MainPage) Platform.Current.Context.Content).DataContext).Result = text;
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
