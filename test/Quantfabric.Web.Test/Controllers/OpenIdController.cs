using System.Threading.Tasks;
using System.Web.Mvc;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
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
            var token = await GetIdToken<Google>("http://localhost:33533/openid/google")
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { idToken = token.ToEncodedWebToken() });
        }

        // GET: openid/yahoo
        [HttpGet]
        public async Task<ActionResult> Google()
        {
            var token = await GetIdToken<Yahoo>("http://quantfabric.com/openid/")
                .ConfigureAwait(false);

            return RedirectToAction("Index", "Home",
                new { idToken = token.ToEncodedWebToken() });
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