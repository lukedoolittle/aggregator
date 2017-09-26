using System.Threading.Tasks;
using System.Web.Mvc;
using Material.Application;
using Material.Contracts;
using Material.Domain.Core;
using Material.Domain.Credentials;
using Material.Domain.Responses;
using Material.Domain.Requests;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;

namespace Quantfabric.Web.Test.Controllers
{
    public class OAuthController : Controller
    {
        private readonly AppCredentialRepository _appRepository;

        public OAuthController()
        {
            _appRepository = new AppCredentialRepository(
                CallbackType.Localhost);
        }

        [HttpGet]
        public async Task<ActionResult> FacebookCallback()
        {
            OAuth2Credentials fullCredentials = await new OAuth2Web<Facebook>(
                    "YOUR CLIENT ID",
                    "YOUR CLIENT SECRET",
                    "HTTP://YOURCALLBACKURI")
                .GetAccessTokenAsync(
                    ControllerContext.HttpContext.Request.Url)
                .ConfigureAwait(false);

            FacebookUserResponse user = await new AuthorizedRequester(fullCredentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            string email = user.Email;
            //Do something with users email address

            return RedirectToAction("Index", "Home");
        }

        // GET: oauth/twitter
        [HttpGet]
        public async Task<ActionResult> Twitter()
        {
            var url = ControllerContext.HttpContext.Request.Url;

            var credentials = await ServiceLocator.TwitterOAuth
                    .GetAccessTokenAsync(
                        url)
                    .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { oauthToken = credentials.OAuthToken, oauthSecret = credentials.OAuthSecret });
        }

        // GET: oauth/Tumblr
        [HttpGet]
        public async Task<ActionResult> Tumblr()
        {
            var credentials = await GetOAuth1Credentials<Tumblr>()
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
            var url = ControllerContext.HttpContext.Request.Url;

           var credentials = await ServiceLocator.FacebookAuth
                    .GetAccessTokenAsync(
                        url)
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

        // GET: oauth/google
        [HttpGet]
        public async Task<ActionResult> Youtube()
        {
            var credentials = await GetOAuth2Credentials<Youtube>()
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

        // GET: oauth/pinterest
        [HttpGet]
        public async Task<ActionResult> Pinterest()
        {
            var credentials = await GetOAuth2Credentials<Pinterest>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/pinterest
        [HttpGet]
        public async Task<ActionResult> Instagram()
        {
            var credentials = await GetOAuth2Credentials<Instagram>()
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { accessToken = credentials.AccessToken });
        }

        // GET: oauth/amazon
        [HttpGet]
        public async Task<ActionResult> Amazon()
        {
            var credentials = await GetOAuth2Credentials<Amazon>()
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

            var url = ControllerContext.HttpContext.Request.Url;

            return oauth
                .GetAccessTokenAsync(
                    url);
        }

        public Task<OAuth2Credentials> GetOAuth2Credentials<TResourceProvider>()
            where TResourceProvider : OAuth2ResourceProvider, new()
        {
            var oauth = new OAuth2Web<TResourceProvider>(
                _appRepository.GetClientId<TResourceProvider>(),
                _appRepository.GetClientSecret<TResourceProvider>(),
                _appRepository.GetRedirectUri<TResourceProvider>());

            var url = ControllerContext.HttpContext.Request.Url;

            return oauth
                .GetAccessTokenAsync(
                    url);
        }
    }
}
