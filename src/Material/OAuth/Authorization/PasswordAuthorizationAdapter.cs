using System;
using System.Linq;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Extensions;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Authorization
{
    public class PasswordAuthorizationAdapter
    {
        public async Task<PasswordCredentials> GetAccessToken(
            Uri tokenUri,
            string username,
            string usernameKey,
            string password,
            string passwordKey)
        {
            var builder = new AuthenticatorBuilder()
                .AddParameter(new Username(
                    username, 
                    usernameKey))
                .AddParameter(new Password(
                    password, 
                    passwordKey));

            var result = (await new HttpRequestBuilder(tokenUri.NonPath())
                .PostTo(tokenUri.AbsolutePath)
                .Authenticator(builder)
                .DisableAutoRedirect()
                .ExecuteAsync()
                .ConfigureAwait(false));

            return new PasswordCredentials()
                .SetCookies(
                    result.Cookies.ToArray());
        }
    }
}
