using System.Threading.Tasks;
using System.Web.Mvc;
using Material.Contracts;
using Material.Facades;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Quantfabric.Test.Helpers;

namespace Quantfabric.Web.Test.Controllers
{
    public class OAuthController : Controller
    {
        private readonly AppCredentialRepository _appRepository;

        public OAuthController()
        {
            _appRepository = new AppCredentialRepository(
                CallbackTypeEnum.Localhost);
        }

        // GET: oauth/twitter
        [HttpGet]
        public async Task<ActionResult> Twitter()
        {
            var credentials = await GetOAuth1Credentials<Twitter>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { oauthToken = credentials.OAuthToken, oauthSecret = credentials.OAuthSecret });
        }

        // GET: oauth/withings
        [HttpGet]
        public async Task<ActionResult> Withings()
        {
            var credentials = await GetOAuth1Credentials<Withings>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { oauthToken = credentials.OAuthToken, oauthSecret = credentials.OAuthSecret });
        }

        // GET: oauth/fatsecret
        [HttpGet]
        public async Task<ActionResult> Fatsecret()
        {
            var credentials = await GetOAuth1Credentials<Fatsecret>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { oauthToken = credentials.OAuthToken, oauthSecret = credentials.OAuthSecret });
        }

        // GET: oauth/facebook
        [HttpGet]
        public async Task<ActionResult> Facebook()
        {
            var credentials = await GetOAuth2Credentials<Facebook>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home", 
                new {accessToken = credentials.AccessToken});
        }

        // GET: oauth/google
        [HttpGet]
        public async Task<ActionResult> Google()
        {
            var credentials = await GetOAuth2Credentials<Google>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/foursquare
        [HttpGet]
        public async Task<ActionResult> Foursquare()
        {
            var credentials = await GetOAuth2Credentials<Foursquare>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/spotify
        [HttpGet]
        public async Task<ActionResult> Spotify()
        {
            var credentials = await GetOAuth2Credentials<Spotify>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/runkeeper
        [HttpGet]
        public async Task<ActionResult> Runkeeper()
        {
            var credentials = await GetOAuth2Credentials<Runkeeper>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/fitbit
        [HttpGet]
        public async Task<ActionResult> Fitbit()
        {
            var credentials = await GetOAuth2Credentials<Fitbit>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/rescuetime
        [HttpGet]
        public async Task<ActionResult> Rescuetime()
        {
            var credentials = await GetOAuth2Credentials<Rescuetime>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/twentythreeandme
        [HttpGet]
        public async Task<ActionResult> TwentyThreeAndMe()
        {
            var credentials = await GetOAuth2Credentials<TwentyThreeAndMe>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/linkedin
        [HttpGet]
        public async Task<ActionResult> Linkedin()
        {
            var credentials = await GetOAuth2Credentials<LinkedIn>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        public Task<OAuth1Credentials> GetOAuth1Credentials<TResourceProvider>()
            where TResourceProvider : OAuth1ResourceProvider, new()
        {
            var oauth = new OAuth1Web<TResourceProvider>(
                _appRepository.GetConsumerKey<TResourceProvider>(),
                _appRepository.GetConsumerSecret<TResourceProvider>(),
                _appRepository.GetRedirectUri<TResourceProvider>());

            var userId = Request.Cookies["userId"]?.Values["userId"];
            var url = ControllerContext.HttpContext.Request.Url;

            var intermediateCredentials = oauth
                .ParseAndValidateCallback(url, userId);

            return oauth
                .GetAccessTokenAsync(
                    intermediateCredentials,
                    _appRepository.GetConsumerSecret<TResourceProvider>());
        }

        public Task<OAuth2Credentials> GetOAuth2Credentials<TResourceProvider>()
            where TResourceProvider : OAuth2ResourceProvider, new()
        {
            var oauth = new OAuth2Web<TResourceProvider>(
                _appRepository.GetClientId<TResourceProvider>(),
                _appRepository.GetRedirectUri<TResourceProvider>());

            var userId = Request.Cookies["userId"]?.Values["userId"];
            var url = ControllerContext.HttpContext.Request.Url;

            var intermediateCredentials = oauth
                .ParseAndValidateCallback(url, userId);

            return oauth
                .GetAccessTokenAsync(
                    intermediateCredentials,
                    _appRepository.GetClientSecret<TResourceProvider>());
        }
    }
}
