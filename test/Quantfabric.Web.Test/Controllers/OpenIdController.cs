using System.Threading.Tasks;
using System.Web.Mvc;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth.Workflow;
using Quantfabric.Test.Helpers;

namespace Quantfabric.Web.Test.Controllers
{
    public class OpenIdController : Controller
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);

        // GET: openid/google
        [HttpGet]
        public async Task<ActionResult> Google()
        {
            var userId = Request.Cookies["userId"]?.Values["userId"];
            var url = ControllerContext.HttpContext.Request.Url;

            var token = await ServiceLocator.GoogleAuth
                    .GetWebTokenAsync(
                        url,
                        userId)
                    .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { idToken = token.EncodedToken });
        }

        // GET: openid/yahoo
        [HttpGet]
        public async Task<ActionResult> Yahoo()
        {
            var token = await GetIdToken<Yahoo>("http://quantfabric.com/openid/")
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { idToken = token.EncodedToken });
        }

        public Task<JsonWebToken> GetIdToken<TResourceProvider>(string uri)
            where TResourceProvider : OpenIdResourceProvider, new()
        {
            var oauth = new OpenIdWeb<TResourceProvider>(
                _appRepository.GetClientId<TResourceProvider>(),
                _appRepository.GetClientSecret<TResourceProvider>(),
                uri);

            var userId = Request.Cookies["userId"]?.Values["userId"];
            var url = ControllerContext.HttpContext.Request.Url;

            return oauth
                .GetWebTokenAsync(
                    url,
                    userId);
        }
    }
}