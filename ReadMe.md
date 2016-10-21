# Quantfabric Material
A cross platform framework for collecting personal data from various sources including web apis (OAuth), bluetooth devices, and smartphone embedded sensors (GPS, SMS, etc).

[![NuGet](https://img.shields.io/nuget/v/Quantfabric.Material.svg?maxAge=2592000)](https://www.nuget.org/packages/Quantfabric.Material/)


## Examples
Authenticate with Facebook on a Mobile/Desktop app:

    OAuth2Credentials credentials = await new OAuth2App<Facebook>(
            "YOUR CLIENT ID", 
            "HTTP://YOURCALLBACKURI")
        .AddScope<FacebookUser>()
        .GetCredentialsAsync()
        .ConfigureAwait(false);

    FacebookUserResponse user = await new OAuthRequester(credentials)
        .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
        .ConfigureAwait(false);

    string email = user.Email;
    //Do something with users email address

Authorize with Facebook in a web app (.NET MVC):

    [HttpGet]
    public async Task<ActionResult> FacebookRedirect()
    {
    	//Or get the userId from your application
        string userId = Guid.NewGuid().ToString();  

        HttpCookie cookie = new HttpCookie("userId");
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
            OAuth2Credentials fullCredentials = await new OAuth2Web<Facebook>(
                    "YOUR CLIENT ID", 
                    "YOUR CLIENT SECRET",
                    "HTTP://YOURCALLBACKURI")
                .GetAccessTokenAsync(
                    ControllerContext.HttpContext.Request.Url,
                    Request.Cookies["userId"]?.Values["userId"],
                    "YOUR CLIENT SECRET")
                .ConfigureAwait(false);

            FacebookUserResponse user = await new OAuthRequester(fullCredentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            string email = user.Email;
            //Do something with users email address
            
            return RedirectToAction("Index", "Home");
        }
    }
    
    
Authenticate with Google on a Mobile/Desktop app:

    OAuth2Credentials credentials = await new OAuth2App<Google>(
            "YOUR CLIENT ID", 
            "HTTP://YOURCALLBACKURI")
        .AddScope<GoogleProfile>()
        .GetCredentialsAsync()
        .ConfigureAwait(false);

    GoogleProfileResponse profile = await new OAuthRequester(credentials)
        .MakeOAuthRequestAsync<GoogleProfile, GoogleProfileResponse>()
        .ConfigureAwait(false);

    string email = profile.Emails.First().Value;
    //Do something with users email address
    
## Documentation
Documentation is available in the [gihub wiki](https://github.com/lukedoolittle/quantfabric/wiki). The documentation covers all scenarios for OAuth1/OAuth2 on Mobile/Desktop/Web, Bluetooth GATT, mobile device sensors, as well as some advanced topics.

## Building
### Developer Environment
* [Visual Studio 2015](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx) (Community Edition is free!)
* [Visual Studio Tools for Xamarin](https://www.xamarin.com/download) (also free now!)
* [Visual Studio Tools for Windows 10](https://developer.microsoft.com/en-us/windows/downloads)
* [T4 Toolbox Extension](https://visualstudiogallery.msdn.microsoft.com/34b6d489-afbc-4d7b-82c3-dded2b726dbc)
* An Apple build machine (definitely not free, but only needed for iOS)

There are automated tests for Windows Console, Xamarin.iOS and Xamarin.Android. There are also manual UI tests for Xamarin.iOS, Xamarin.Android, UWP and .NET MVC.

## Contributing
All contributions are welcome! The only requirement is some test coverage of any code change. Also the static code analyzer runs on a few of the projects so check build warnings for any changes. Quantfabric has a [waffle.io](https://waffle.io/lukedoolittle/quantfabric) site for viewing and managing issues as well as a [slack channel](https://quantfabric.slack.com/) for any discussions.