using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;

namespace Material.Application
{
    public class SimplePassword<TResourceProvider>
        where TResourceProvider : PasswordResourceProvider, new()
    {
        private readonly string _username;
        private readonly string _password;

        public SimplePassword(
            string username, 
            string password)
        {
            _username = username;
            _password = password;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<PasswordCredentials> GetCredentialsAsync()
        {
            var provider = new TResourceProvider();

            return new PasswordAuthorizationAdapter()
                .GetAccessToken(
                provider.TokenUrl,
                _username, 
                provider.UsernameKey,
                _password, 
                provider.PasswordKey);
        }
    }
}
