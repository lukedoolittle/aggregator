using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Material.Application;
using Material.Contracts;
using Material.Domain.Core;
using Material.Domain.Requests;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;

namespace Quantfabric.Web.Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppCredentialRepository _appRepository;

        public HomeController()
        {
            _appRepository = new AppCredentialRepository(
                CallbackType.Localhost);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> FacebookRedirect()
        {
            Uri authorizationUri = await new OAuth2Web<Facebook>(
                    "YOUR CLIENT ID",
                    "YOUR CLIENT SECRET",
                    "HTTP://YOURCALLBACKURI")
                .AddScope<FacebookUser>()
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(authorizationUri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Tumblr()
        {
            var uri = await GetOAuth1AuthorizationUri<Tumblr>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Twitter()
        {
            var uri = await ServiceLocator
                .TwitterOAuth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Withings()
        {
            var uri = await GetOAuth1AuthorizationUri<Withings>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Fatsecret()
        {
            var uri = await GetOAuth1AuthorizationUri<Fatsecret>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Facebook()
        {
            var uri = await ServiceLocator
                .FacebookAuth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Foursquare()
        {
            var uri = await GetOAuth2AuthorizationUri<Foursquare>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Google()
        {
            var oauth = new OAuth2Web<Google>(
                _appRepository.GetClientId<Google>(),
                _appRepository.GetClientSecret<Google>(),
                _appRepository.GetRedirectUri<Google>())
                .AddScope<GoogleGmailMetadata>();

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Pinterest()
        {
            var oauth = new OAuth2Web<Pinterest>(
                _appRepository.GetClientId<Pinterest>(),
                _appRepository.GetClientSecret<Pinterest>(),
                _appRepository.GetRedirectUri<Pinterest>())
                .AddScope<PinterestFollowers>()
                .AddScope<PinterestFollowing>()
                .AddScope<PinterestPins>();

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Spotify()
        {
            var uri = await GetOAuth2AuthorizationUri<Spotify>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Runkeeper()
        {
            var uri = await GetOAuth2AuthorizationUri<Runkeeper>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Linkedin()
        {
            var uri = await GetOAuth2AuthorizationUri<LinkedIn>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Fitbit()
        {
            var oauth = new OAuth2Web<Fitbit>(
                _appRepository.GetClientId<Fitbit>(),
                _appRepository.GetClientSecret<Fitbit>(),
                _appRepository.GetRedirectUri<Fitbit>())
                .AddScope<FitbitProfile>()
                .AddScope<FitbitSleep>();

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Rescuetime()
        {
            var uri = await GetOAuth2AuthorizationUri<Rescuetime>()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> TwentyThreeAndMe()
        {
            var oauth = new OAuth2Web<TwentyThreeAndMe>(
                _appRepository.GetClientId<TwentyThreeAndMe>(),
                _appRepository.GetClientSecret<TwentyThreeAndMe>(),
                _appRepository.GetRedirectUri<TwentyThreeAndMe>())
                .AddScope<TwentyThreeAndMeUser>();

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Instagram()
        {
            var oauth = new OAuth2Web<Instagram>(
                _appRepository.GetClientId<Instagram>(),
                _appRepository.GetClientSecret<Instagram>(),
                _appRepository.GetRedirectUri<Instagram>())
                .AddScope<InstagramUser>()
                .AddScope<InstagramLikes>();

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Amazon()
        {
            var oauth = new OAuth2Web<Amazon>(
                _appRepository.GetClientId<Amazon>(),
                _appRepository.GetClientSecret<Amazon>(),
                _appRepository.GetRedirectUri<Amazon>())
                .AddScope<AmazonProfile>();

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> GoogleOpenId()
        {
            var uri = await ServiceLocator.GoogleAuth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> YahooOpenId()
        {
            var oauth = new OpenIdWeb<Yahoo>(
                _appRepository.GetClientId<Yahoo>(),
                _appRepository.GetClientSecret<Yahoo>(),
                "http://quantfabric.com/openid/");

            var uri = await oauth
                .GetAuthorizationUriAsync()
                .ConfigureAwait(false);

            return Redirect(uri.ToString());
        }

        private Task<Uri> GetOAuth1AuthorizationUri<TProtectedResource>()
            where TProtectedResource : OAuth1ResourceProvider, new()
        {
            var oauth = new OAuth1Web<TProtectedResource>(
                _appRepository.GetConsumerKey<TProtectedResource>(),
                _appRepository.GetConsumerSecret<TProtectedResource>(),
                _appRepository.GetRedirectUri<TProtectedResource>());

            return oauth.GetAuthorizationUriAsync();
        }

        private Task<Uri> GetOAuth2AuthorizationUri<TProtectedResource>()
            where TProtectedResource : OAuth2ResourceProvider, new()
        {
            var oauth = new OAuth2Web<TProtectedResource>(
                _appRepository.GetClientId<TProtectedResource>(),
                _appRepository.GetClientSecret<TProtectedResource>(),
                _appRepository.GetRedirectUri<TProtectedResource>());

            return oauth.GetAuthorizationUriAsync();
        }
    }
}