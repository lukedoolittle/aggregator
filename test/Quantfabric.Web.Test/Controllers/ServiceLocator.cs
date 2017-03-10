using Material.Application;
using Material.Contracts;
using Material.Infrastructure.ProtectedResources;
using Quantfabric.Test.Helpers;

namespace Quantfabric.Web.Test.Controllers
{
    public class ServiceLocator
    {
        private static readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);

        public static OAuth1Web<Twitter> TwitterOAuth { get; } = 
            new OAuth1Web<Twitter>(
                _appRepository.GetConsumerKey<Twitter>(),
                _appRepository.GetConsumerSecret<Twitter>(),
                _appRepository.GetRedirectUri<Twitter>());

        public static OAuth2Web<Facebook> FacebookAuth { get; } = 
            new OAuth2Web<Facebook>(
                _appRepository.GetClientId<Facebook>(),
                _appRepository.GetClientSecret<Facebook>(),
                _appRepository.GetRedirectUri<Facebook>());

        public static OpenIdWeb<Google> GoogleAuth { get; } =
             new OpenIdWeb<Google>(
                 _appRepository.GetClientId<Google>(),
                _appRepository.GetClientSecret<Google>(),
                "http://localhost:33533/openid/google");
    }
}