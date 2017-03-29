using System;
using Material.Application;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Framework.Enums;
using Material.Domain.Requests;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace Quantfabric.UI.Test.Forms
{
    public partial class TestList : ContentPage
    {
        private AuthorizationInterface _browserType =
            AuthorizationInterface.NotSpecified;

        private CallbackType _callbackType =
            CallbackType.NotSpecified;

        public TestList()
        {
            InitializeComponent();
        }

        private async void FacebookButton_OnClicked(object sender, EventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Facebook>();
            var clientSecret = settings.GetClientSecret<Facebook>();
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

        private async void GoogleButton_OnClicked(object sender, EventArgs e)
        {
            var settings = new AppCredentialRepository(_callbackType);
            var clientId = settings.GetClientId<Google>();
            var clientSecret = settings.GetClientSecret<Google>();
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

        private void FoursquareButton_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SpotifyButton_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            Device.BeginInvokeOnMainThread(() =>
            {
                ((TestList) App.Current.MainPage).ResultTextBox.Text = text;
            });
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as Picker).SelectedIndex)
            {
                case 0:
                    _browserType = AuthorizationInterface.NotSpecified;
                    _callbackType = CallbackType.NotSpecified;
                    break;
                case 1:
                    _browserType = AuthorizationInterface.Embedded;
                    _callbackType = CallbackType.Localhost;
                    break;
                case 2:
                    _browserType = AuthorizationInterface.Dedicated;
                    _callbackType = CallbackType.Protocol;
                    break;
            }
        }
    }
}
