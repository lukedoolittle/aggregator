using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Material.Contracts;
using Material.Facades;
using Material.Infrastructure;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;

namespace Quantfabric.Web.Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppCredentialRepository _appRepository;

        public HomeController()
        {
            _appRepository = new AppCredentialRepository(
                CallbackTypeEnum.Localhost);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Twitter()
        {
            var uri = await GetOAuth1AuthorizationUri<Twitter>()
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
            var uri = await GetOAuth2AuthorizationUri<Facebook>()
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
                _appRepository.GetRedirectUri<Google>())
                .AddScope<GoogleGmailMetadata>();

            var uri = await GetOAuth2AuthorizationUri(oauth)
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
            var uri = await GetOAuth2AuthorizationUri<Fitbit>()
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
            var uri = await GetOAuth2AuthorizationUri<TwentyThreeAndMe>()
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

            var userId = Guid.NewGuid().ToString();
            var cookie = new HttpCookie("userId");
            cookie.Values["userId"] = userId;
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);

            return oauth
                .GetAuthorizationUriAsync(userId);
        }

        private Task<Uri> GetOAuth2AuthorizationUri<TProtectedResource>()
            where TProtectedResource : OAuth2ResourceProvider, new()
        {
            var oauth = new OAuth2Web<TProtectedResource>(
                _appRepository.GetClientId<TProtectedResource>(),
                _appRepository.GetRedirectUri<TProtectedResource>());

            return GetOAuth2AuthorizationUri(oauth);
        }

        private Task<Uri> GetOAuth2AuthorizationUri<TProtectedResource>(
            OAuth2Web<TProtectedResource> oauth)
            where TProtectedResource : OAuth2ResourceProvider, new()
        {
            var userId = Guid.NewGuid().ToString();
            var cookie = new HttpCookie("userId");
            cookie.Values["userId"] = userId;
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);

            return oauth
                .GetAuthorizationUriAsync(userId);
        }
    }
}