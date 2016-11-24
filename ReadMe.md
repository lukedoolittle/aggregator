# Quantfabric Material
A cross platform framework for collecting personal data from various sources.
* Authorize and make requests against more than 20 APIs on Android, iOS, UWP, .NET MVC and traditional Windows apps
* Authenticate with OpenId Connect
* Exchange Authorization credentials for Authentication credentials for any API (just like [Auth0](https://auth0.com/))
* Collect data from mobile sensors (GPS, SMS)
* Interface with any GATT supported bluetooth device without any UI work

[![NuGet](https://img.shields.io/nuget/v/Quantfabric.Material.svg?maxAge=2592000)](https://www.nuget.org/packages/Quantfabric.Material/)

## Documentation
Documentation is available in the [gihub wiki](https://github.com/lukedoolittle/quantfabric/wiki). The documentation covers scenarios for OAuth1/OAuth2 on Mobile/Desktop/Web, Bluetooth GATT, mobile device sensors, as well as some advanced topics.

## Simple Examples
Authorize a user with Facebook on a Mobile/Desktop app:

    OAuth2Credentials credentials = await new OAuth2App<Facebook>(
            "YOUR CLIENT ID", 
            "HTTP://YOURCALLBACKURI")
        .AddScope<FacebookUser>()
        .GetCredentialsAsync()
        .ConfigureAwait(false);

    FacebookUserResponse user = await new OAuthRequester(credentials)
        .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
        .ConfigureAwait(false);

    string userEmail = user.Email;


Authorize with Facebook in a web app (.NET MVC):

    [HttpGet]
    public async Task<ActionResult> FacebookRedirect()
    {
    	//Or get the userId from your application
        string userId = Guid.NewGuid().ToString();  

        HttpCookie cookie = new HttpCookie("userCookie");
        cookie.Values["userId"] = userId;
        ControllerContext.HttpContext.Response.Cookies.Add(cookie);

        Uri authorizationUri = await new OAuth2Web<Facebook>(
                "YOUR CLIENT ID", 
                "YOUR CLIENT SECRET",
                "HTTP://YOURCALLBACKURI")
            .AddScope<FacebookUser>()
            .GetAuthorizationUriAsync(userId)
            .ConfigureAwait(false);

        return Redirect(authorizationUri.ToString());
    }

Handle a Facebook OAuth callback in a web app (.NET MVC)

    public class OAuthController : Controller
    {
        // GET: oauth/facebookcallback
        [HttpGet]
        public async Task<ActionResult> FacebookCallback()
        {
            HttpCookie cookie = Request.Cookies["userCookie"];

            OAuth2Credentials fullCredentials = await new OAuth2Web<Facebook>(
                    "YOUR CLIENT ID", 
                    "YOUR CLIENT SECRET",
                    "HTTP://YOURCALLBACKURI")
                .GetAccessTokenAsync(
                    ControllerContext.HttpContext.Request.Url,
                    cookie.Values["userId"],
                    "YOUR CLIENT SECRET")
                .ConfigureAwait(false);

            FacebookUserResponse user = await new OAuthRequester(fullCredentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            string userEmail = user.Email;
            cookie.Values["userEmail"] = usersEmail;
            
            return RedirectToAction("Index", "Home");
        }
    }
    
    
Authenticate with Google on a Mobile/Desktop app:

    JsonWebToken token = await new OpenIdApp<Google>(
            "YOUR CLIENT ID", 
            "HTTP://YOURCALLBACKURI")
        .GetWebTokenAsync()
        .ConfigureAwait(false);

    string userId = token.Claims.Subject;
    
## Building
### Developer Environment
* [Visual Studio 2015](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx) (Community Edition is free!)
* [Visual Studio Tools for Xamarin](https://www.xamarin.com/download) (also free now!)
* [Visual Studio Tools for Windows 10](https://developer.microsoft.com/en-us/windows/downloads) (always free)
* [T4 Toolbox Extension](https://visualstudiogallery.msdn.microsoft.com/34b6d489-afbc-4d7b-82c3-dded2b726dbc) (yep, this one is free too)
* An Apple build machine (definitely not free, but only needed for iOS)

There are automated tests for Windows Console. There are also manual UI tests for Xamarin.iOS, Xamarin.Android, UWP and .NET MVC.

## Contributing
All contributions are welcome! The only requirement is some test coverage of any code change and review of any static code analyzer warnings. Quantfabric has a [waffle.io](https://waffle.io/lukedoolittle/quantfabric) site for viewing and managing issues as well as a [slack channel](https://quantfabric.slack.com/) for any discussions.