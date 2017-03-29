using System;
using Material.Domain.Credentials;
using System.Linq;
using System.Threading.Tasks;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Extensions;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Authorization
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
