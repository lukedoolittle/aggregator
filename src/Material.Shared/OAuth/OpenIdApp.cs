using System;
using System.Threading.Tasks;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OpenIdApp<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly OAuth2App<TResourceProvider> _app;

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
            AuthorizationInterface browserType = AuthorizationInterface.Dedicated
#else
            AuthorizationInterface browserType = AuthorizationInterface.Embedded
#endif
            )
        {
             _app = new OAuth2App<TResourceProvider>(
                clientId,
                callbackUrl,
                provider,
                browserType);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
#if __WINDOWS__
            AuthorizationInterface browserType = AuthorizationInterface.Dedicated
#else
            AuthorizationInterface browserType = AuthorizationInterface.Embedded
#endif
            ) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetCredentialsAsync(
            string clientSecret)
        {
            var credentials = await _app
                .GetCredentialsAsync(clientSecret)
                .ConfigureAwait(false);

            return await ExtractAndValidateAuthenticationToken(credentials)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetCredentialsAsync()
        {
            var credentials = await _app.GetCredentialsAsync()
                .ConfigureAwait(false);

            return await ExtractAndValidateAuthenticationToken(credentials)
                .ConfigureAwait(false);
        }

        private async Task<JsonWebToken> ExtractAndValidateAuthenticationToken(
            OAuth2Credentials credentials)
        {
            //TODO: do we also need to pass the validation endpoint here???
            //How do any clients know where to go to validate the response???
            //TODO: pull the webtoken out of the credentials response, get the key from the discovery endpoint and validate
            throw new NotImplementedException();
        }
    }
}
