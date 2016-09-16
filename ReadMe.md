# Quantfabric Material
A cross platform framework for collecting personal data from various sources including web apis (OAuth), bluetooth devices, and smartphone embedded sensors (GPS, SMS, etc)

# NuGet
You can get Material by [grabbing the latest NuGet package](#https://www.nuget.org/packages/Quantfabric.Material/) currently available for .NET 4.5, Xamarin.Android, Xamarin.iOS, and Windows UWP (Xamarin.Forms in progress)

## What is your usage scenario?
1. [I have a mobile/desktop app and I want to use an OAuth1 authentication provider (ie Login with Twitter)](#oauth1_app)
2. [I have a mobile/desktop app and I want to use an OAuth2 authentication provider (ie Login with Facebook)](#oauth2_app)
3. [I have a web application and I want to use an OAuth1 authentication provider (ie Login with Twitter)](#oauth1_web)
4. [I have a web application and I want to use an OAuth2 authentication provider (ie Login with Facebook)](#oauth2_web)
5. [I have a mobile, web or desktop app and I want to refresh my OAuth2 credentials](#oauth2_refresh)
6. [I have a mobile, web or desktop app and I want to access OAuth protected resources (ie access to my users Tweets)](#protected_resources)
7. [I want to test OAuth2 authentication workflows for an OAuth2 provider I am creating](#oauth2_app)
8. [I want to connect to a Bluetooth GATT device](#bluetooth) 
9. [I want to consume a device resource (SMS or GPS)](#device)

## <a name="oauth1_providers"></a> List of OAuth1 Providers
* `Twitter`
* `Fatsecret`
* `Withings`
* [Create a Provider](#advanced_provider)

## <a name="oauth2_providers"></a> List of OAuth2 Providers
* `Facebook` (token expires, no refresh token provided)
* `Fitbit` (token expires, refresh token provided)
* `Foursquare` (token does not expire)
* [`Google`](#google) (token expires, refresh token provided)
* `LinkedIn` (token does not expire)
* [`Rescuetime`](#rescuetime) (token does not expire)
* `Runkeeper` (token does not expire)
* `Spotify` (token expires, refresh token provided)
* `TwentyThreeAndMe` (token expires, refresh token provided)
* [`Pinterest`](#pinterest) (token does not expire)
* `Instagram` (token does not expire)
* [Create a Provider](#advanced_provider)

## List of Requests
* `FacebookFeed`
* `FacebookEvent`
* `FacebookFriend`
* `FacebookPageLike`
* `FatsecretMeal`
* `FitbitIntradayHeartRate`
* `FitbitIntradayHeartRateBulk`
* `FitbitIntradaySteps`
* `FitbitIntradayStepsBulk`
* `FitbitProfile`
* `FitbitSleep`
* `FoursquareCheckin`
* `FoursquareFriend`
* `FoursquareTip`
* `GoogleGmail`
* `GoogleGmailMetadata`
* `LinkedinPersonal`
* `LinkedinUpdate`
* `RescuetimeAnalyticData`
* `RunkeeperFitnessActivity`
* `SpotifySavedTrack`
* `TwentyThreeAndMeGenome`
* `TwentyThreeAndMeUser`
* `TwitterFavorite`
* `TwitterFollower`
* `TwitterFollowing`
* `TwitterMention`
* `TwitterReceivedDirectMessage`
* `TwitterSentDirectMessage`
* `TwitterRetweetOfMe`
* `TwitterTimeline`
* `TwitterTweet`
* `WithingsWeighin`
* `InstagramLikes`
* `PinterestLikes`
* [Create a Request](#advanced_request)

## <a name="oauth1_web"></a> Web App Authentication Provider (OAuth1)
To obtain the redirect url for authorization on the resource providers server:

	//OPTIONALLY: inject the OAuth1Web instance into your class or method
	string consumerKey = "YOUR CONSUMER KEY";
    string consumerSecret = "YOUR CONSUMER SECRET"
	string callbackUri = "HTTP://YOURCALLBACKURI";
    
	OAuth1Web<Twitter> oauth = new OAuth1Web<Twitter>(
		consumerKey, 
		consumerSecret, 
		callbackUri);
	
    string userId = "SOMEUSERID";  //some unique identifier stored in a cookie or session state
    
	Uri twitterLoginEndpoint = await oauth
		.GetAuthorizationUriAsync(userId)
		.ConfigureAwait(false);

To handle the callback to your server (assume this callback is an endpoint `TwitterCallback` within an .NET MVC `Controller`):

	public async Task<ActionResult> TwitterCallback()
	{
    	//SUGGESTED: inject the OAuth1Web instance into your controller
		string clientId = "YOUR CLIENT ID";
		string clientSecret = "YOUR CLIENT SECRET";
		string callbackUri = "HTTP://YOURCALLBACKURI";
        
		OAuth1Web<Twitter> oauth = new OAuth1Web<Twitter>(
			clientId, 
			clientSecret,
			callbackUri);
	
    	string userId = Request.Cookies["userId"];
    	string url = ControllerContext.HttpContext.Request.Url;
        
    	OAuth1Credentials intermediateCredentials = oauth
			.ParseAndValidateCallback(url, userId);
    		
		OAuth1Credentials credentials = await oauth
			.GetAccessTokenAsync(intermediateCredentials)
			.ConfigureAwait(false);
		
		//you've got the credentials!

		return RedirectToAction("SOMEREDIRECTPAGE"); 
	}
    
\* If you are working in an environment with multiple servers and you do not use sticky sessions, see the advanced topic [Creating Your Own Security Strategy](#advanced_security)

## <a name="oauth2_web"></a> Web App Authentication Provider (OAuth2)
To obtain the redirect url for authorization on the resource providers server:

	//OPTIONALLY: inject the OAuth2Web instance into your class or method
	string clientId = "YOUR CLIENT ID";
	string callbackUri = "HTTP://YOURCALLBACKURI";
	
	OAuth2Web<Facebook> oauth = new OAuth2Web<Facebook>(
		clientId,  
		callbackUri);
        
	string userId = "SOMEUSERID";  //some unique identifier stored in a cookie or session state
    
	Uri facebookLoginEndpoint = await oauth
		.GetAuthorizationUriAsync(userId)
		.ConfigureAwait(false);

If you want to make a subsequent request for a protected resource add scopes for each request you intend to make. For example:

	Uri facebookLoginEndpoint = await oauth
		.AddScope<FacebookFeed>()
		.AddScope<FacebookFriend>()
		.GetAuthorizationUriAsync(userId)
		.ConfigureAwait(false);

To handle the callback to your server (assume this callback is an endpoint `FacebookCallback` within an .NET MVC `Controller` and that the userId is stored in a cookie):

	public async Task<ActionResult> FacebookCallback()
	{
    	//SUGGESTED: inject the OAuth2Web instance into your controller
		string clientId = "YOUR CLIENT ID";
		string clientSecret = "YOUR CLIENT SECRET";
        string callbackUri = "HTTP://YOURCALLBACKURI";
		
		OAuth2Web<Facebook> oauth = new OAuth2Web<Facebook>(
			clientId, 
			clientSecret, 
			callbackUri);

		var userId = Request.Cookies["userId"]?.Value;
		var url = ControllerContext.HttpContext.Request.Url;
        
		OAuth2Credentials intermediateCredentials = oauth
			.ParseAndValidateCallback(url, userId);
		
		OAuth2Credentials credentials = await oauth
			.GetAccessTokenAsync(intermediateCredentials)
			.ConfigureAwait(false);
		
		//you've got the credentials!

		return RedirectToAction("SOMEREDIRECTPAGE"); 
	}

\* If you are working in an environment with multiple servers and you do not use sticky sessions, see the advanced topic [Creating Your Own Security Strategy](#advanced_security)

## <a name="oauth1_app"></a> Mobile/Desktop Authentication Provider (OAuth1)
When creating your app with the resource provider use localhost as the callback uri with an uncommon port number. For example `http://localhost:33533/twitter`

	public async Task MyAuthenticationMethod()
	{
		string consumerKey = "YOUR CONSUMER KEY";
		string consumerSecret = "YOUR CONSUMER SECRET";
		string callbackUri = "http://localhost:33533/twitter";
		
		OAuth1App<Twitter> oauth1 = new OAuth1App<Twitter>(
			consumerKey, 
			consumerSecret, 
			callbackUri);
			
		OAuth1Credentials credentials = await oauth1
			.GetCredentialsAsync()
			.ConfigureAwait(false);
	}

## <a name="oauth2_app"></a> Mobile/Desktop Authentication Provider or for Testing (OAuth2)
When creating your app with the resource provider use localhost as the callback uri with an uncommon port number. For example `http://localhost:33533/facebook`

	public async Task MyAuthenticationMethod()
	{
		string clientId = "YOUR CLIENT ID";
		string callbackUri = "http://localhost:33533/facebook";
		
		OAuth2App<Facebook> oauth2 = new OAuth2App<Facebook>(
			clientId, 
			callbackUri);
		
		OAuth2Credentials credentials = await oauth2
			.GetCredentialsAsync()
			.ConfigureAwait(false);
	}

For subsequent requests of protected resource add scopes for each request type. For example:

	OAuth2Credentials credentials = await oauth
		.AddScope<FacebookFeed>()
		.AddScope<FacebookFriend>()
		.GetCredentialsAsync()
		.ConfigureAwait(false);

The 'code' workflow can also be used in the event a long lived access token is desired and the client secret can be embedded in the application (testing purposes, etc):

	public async Task MyAuthenticationMethod()
	{
		string clientId = "YOUR CLIENT ID";
		string clientSecret = "YOUR CLIENT SECRET";
		string callbackUri = "HTTP://YOURCALLBACKURI";
		
		OAuth2App<Facebook> oauth2 = new OAuth2App<Facebook>(
			clientId, 
			clientSecret, 
			callbackUri);
		
		OAuth2Credentials credentials = await oauth2
			.GetCredentialsAsync()
			.ConfigureAwait(false);
	}

By default `OAuth1App` and `OAuth2App` use an embedded browser (`WebView` on Android, `UIWebView` on iOS, `WebView` on UWP) to complete the oauth workflow. To use a dedicated (system) browser see the section on [Dedicated Mobile Browsers](#advanced_dedicated_browser).

## <a name="oauth2_refresh"></a> OAuth2 Refresh Token
If the authentication provider has an access token that expires and provides a refresh token (see [list of OAuth2 providers](#oauth2_providers)), that refresh token can be exchanged for a new, valid access token:

	OAuth2Credentials googleCredentials = CREDENTIALS_I_GOT_EARLIER;

	if (googleCredentials.IsTokenExpired)
    {
		googleCredentials = await new OAuth2Refresh<Google>()
                						.RefreshCredentialsAsync(
                    						expiredToken)
                						.ConfigureAwait(false);
    }

## <a name="protected_resources"></a> Accessing Protected Resources
After gathering the necessary `OAuth1Credentials` by using either the [OAuth 1 web](#two) or [OAuth 1 desktop](#one) workflows, or `OAuth2Credentials` by using either the [OAuth 2 web](#four) or [OAuth 2 desktop](#three) workflows, a request for a protected resource can be made.

	OAuth1Credentials twitterCredentials = CREDENTIALS_I_GOT_EARLIER;
    
    var response = await new OAuthRequester(twitterCredentials)
    	.MakeOAuthRequestAsync<TwitterTweet, TwitterTweetResponse>()
    	.ConfigureAwait(false);

If the request needs to be customized an instance of the request class can be created

	OAuth1Credentials twitterCredentials = CREDENTIALS_I_GOT_EARLIER;
    
    var request = new TwitterTweet();
    request.Count = 100;
    var response = await new OAuthRequester(twitterCredentials)
    	.MakeOAuthRequestAsync<TwitterTweet, TwitterTweetResponse>(request)
    	.ConfigureAwait(false);

## <a name="bluetooth"></a> Bluetooth

	BluetoothCredentials credentials = await new BluetoothApp<Mioalpha>()
                    						.GetBluetoothCredentialsAsync()
                    						.ConfigureAwait(false);
                                            
    BluetoothResponse result = await new BluetoothRequester()
                    				.MakeBluetoothRequestAsync<MioHeartRate>(credentials)
                    				.ConfigureAwait(false);

To connect to a custom Bluetooth GATT device, see [Creating a Bluetooth Provider and Request](#advanced_bluetooth)

## <a name="device"></a> Device (SMS/GPS)
To request the current GPS position:

	GPSResponse result = await new GPSRequester()
							.MakeGPSRequestAsync()
                    		.ConfigureAwait(false);
                            
(Android only) To request a list of SMS currently in inbox or sent:

	SMSResponse results = await new SMSRequester()
                    		.MakeSMSRequestAsync()
                    		.ConfigureAwait(false);
                            
The SMS results can also be filtered by a date:

	DateTime dateFilter = System.DateTime.Today;
    SMSResponse results = await new SMSRequester()
                    		.MakeSMSRequestAsync(dateFilter)
                    		.ConfigureAwait(false);


## Provider Specific Notes
### <a name="rescuetime"></a> Rescuetime
Since rescuetime requires an HTTPS endpoint and the current HttpServer implementation does not handle HTTPS you will see an error when your Rescuetime callback request comes back, when using a desktop workflow. The current workaround is for the user to manually update the url in the browser window, changing HTTPS into HTTP and then hitting 'return'.

### <a name="pinterest"></a> Pinterest
Similar to Rescuetime above Pinterest requires an HTTPS endpoint and thus you have to manually edit the url that appears in your desktops browser, changing the HTTPS to HTTP.

### <a name="google"></a> Google
When creating credentials for a dedicated browser create a "OAuth client Id" for the "iOS" app type. In the "bundle name" field enter the scheme you have set for your callback. When instantiating the `OAuth2App` class your callback uri should only contain one / after the scheme, not two. For example if you put `myapp` as your bundle name in the credential configuration, then your callback uri should look like `myapp:/something/`.

## Advanced Topics
### <a name="advanced_dedicated_browser"></a> Using a dedicated (system) browser on a mobile device (Android, iOS, UWP)

In some mobile device situations a dedicated browser (Chrome on Android, Safari on iOS, Edge on Windows) may be desired for the workflow. If that is the case an optional parameter can be passed to indicate the browser type. <b>At current this method is only known to work with Google and Fitbit. It requires particular setup steps in the configurations of the iOS, Android, or UWP project to properly receive the protocol based callback. Please read both the below section for your platform and read the above Provider Specific Notes for your service</b>:

		OAuth1App<Twitter> oauth1 = new OAuth1App<Twitter>(
			consumerKey, 
			consumerSecret, 
			callbackUri,
            AuthenticationInterfaceEnum.Dedicated);

		OAuth2App<Facebook> oauth2 = new OAuth2App<Facebook>(
			clientId, 
			clientSecret, 
			callbackUri,
            AuthenticationInterfaceEnum.Dedicated);

<b><i>UWP</i></b>
* Add a Protocol (Custom Scheme) to your Package.appxmanifest through the GUI in Visual Studio or by editing the Package.appxmanifest file directly

		<uap:Extension Category="windows.protocol">
			<uap:Protocol Name="CALLBACK_SCHEME_HERE">
			</uap:Protocol>
		</uap:Extension>
    
* Add a call to `Material.Framework.Platform.Current.Protocol(uri)` in the `OnActivated()` method of your `Application` class

		protected override void OnActivated(IActivatedEventArgs args)
		{
			if (args.Kind == ActivationKind.Protocol)
			{
                ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)args;
                Uri uri = protocolArgs.Uri;

                Material.Framework.Platform.Current.Protocol(uri);

                var frame = Window.Current.Content as Frame;
                if (frame == null)
                frame = new Frame();

                frame.Navigate(typeof(MainPage), uri);
                Window.Current.Content = frame;
                Window.Current.Activate();
			}
		}
        
<b><i>Android</i></b>
* Specify an intent filter by either adding to the AndroidManifest.xml OR adding metadata to your callback activity

		<activity android:label="MAIN_ACTIVITY_LABEL_HERE">
			<intent-filter>
				<action android:name="android.intent.action.VIEW" />
				<category android:name="android.intent.category.DEFAULT" />
				<category android:name="android.intent.category.BROWSABLE" />
				<data android:scheme="CALLBACK_SCHEME_HERE"/>
			</intent-filter>
		</activity>

      [IntentFilter(new[] { Android.Content.Intent.ActionView },
      Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
      DataScheme = "CALLBACK_SCHEME_HERE")]
      
* Add a call to `Material.Framework.Platform.Current.Protocol(uri)` in the `OnActivityCreated()` method of your `MainApplication` class

      public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
      {
          Material.Framework.Platform.Current.Context = activity;

          var data = activity.Intent?.Data;
          if (data != null)
          {
              Material.Framework.Platform.Current.Protocol(
              new Uri(data.ToString()));
          }
      }

<b><i>iOS</i></b>

* Specify a URL scheme in the Info.plist

      <key>CFBundleURLTypes</key>
      <array>
          <dict>
              <key>CFBundleURLName</key>
              <string>SOME_NAME</string>
              <key>CFBundleURLSchemes</key>
              <array>
                  <string>CALLBACK_SCHEME_HERE</string>
              </array>
          </dict>
      </array>

* Add a call to `Material.Framework.Platform.Current.Protocol(uri)` in the `OpenUrl()` method of your `AppDelegate` class 

      public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
      {
          Material.Framework.Platform.Current.Protocol(url);
          return true;
      }
        
### <a name="advanced_security"></a> Creating your own security parameter repository
During the OAuth2 workflow a `InMemoryCryptographicParameterRepository` object is used to store the "state" parameter that is round-tripped to the resource provider. This implementation stores the generated parameters in a static variable in the current app domain. This is problematic in a multi-server scenario without sticky sessions. To remedy this create an implementation of `ICryptographicParameterRepository` that utilizes some other mechanism of storing the parameters (database session cache, cookies, etc). For example:

    public class CookieCryptographicParameterRepository : 
        ICryptographicParameterRepository
    {
        private readonly HttpCookieCollection _cookies;

        public CookieCryptographicParameterRepository(
            HttpCookieCollection cookies)
        {
            _cookies = cookies;
        }

        public void SetCryptographicParameterValue(
            string userId, 
            string parameterName, 
            string parameterValue, 
            DateTimeOffset timestamp)
        {
            var cookie = new HttpCookie(userId + parameterName);
            cookie.Values["value"] = parameterValue;
            cookie.Values["timestamp"] = timestamp.ToString();

            _cookies.Add(cookie);
        }

        public Tuple<string, DateTimeOffset> GetCryptographicParameterValue(
            string userId,
            string parameterName)
        {
            var cookie = _cookies[userId + parameterName];

            if (cookie == null)
            {
                return default(Tuple<string, DateTimeOffset>);
            }
            else
            {
                return new Tuple<string, DateTimeOffset>(
                    cookie["value"], 
                    DateTimeOffset.Parse("timestamp"));
            }
        }

        public void DeleteCryptographicParameterValue(
            string userId, 
            string parameterName)
        {
            _cookies.Remove(userId + parameterName);
        }
    }

When creating an instance of `OAuth2WebFacade`, pass an instance of the repository to `OAuthSecurityStrategy` and then into the facade:

	string clientId = "YOUR CLIENT ID";
	string clientSecret = "YOUR CLIENT SECRET";
	string callbackUri = "HTTP://YOURCALLBACKURI";
	string userId = "SOMEUSERID";
	OAuthSecurityStrategy strategy = new OAuthSecurityStrategy(
                        new CookieCryptographicParameterRepository(
	                        HttpContext.Response.Cookies), 
                        TimeSpan.FromMinutes(2)))
	
	OAuth2WebFacade<Facebook> oauth2 = new OAuth2WebFacade<Facebook>(
		clientId, 
		clientSecret, 
		userId,
		callbackUri,
		strategy); 

### <a name="advanced_bluetooth"></a> Creating a Bluetooth Provider and Request
Create a resource provider class to acquire the device address:

	using Material.Infrastructure.Credentials;
	using Material.Metadata;

	namespace Material.Infrastructure.ProtectedResources
	{
		[CredentialType(typeof(BluetoothCredentials))]        
		public partial class MyBluetoothDevice : BluetoothResourceProvider
		{
		}
	}
    
Create a request to access the GATT characteristic. The static classes `BluetoothServices` and `BluetoothCharacteristics` contain the assigned numbers for all approved bluetooth services and characteristics respectively. A conversion function should be written on a characteristic by characteristic basis to convert the raw `byte[]` reading into a string.

    using System;
    using Material.Infrastructure.Bluetooth;
    using Material.Infrastructure.ProtectedResources;
    using Material.Metadata;

    namespace Material.Infrastructure.Requests
    {       
        [ServiceType(typeof(MyBluetoothDevice))]        
        public partial class MyBluetoothDeviceBloodPressureRate : BluetoothRequest
        {
            public override BluetoothSpecification Characteristic => 
                BluetoothCharacteristics.BloodPressureMeasurement;

            public override Func<byte[], string> CharacteristicConverter =>
                (data) => data.ToString();

            public override BluetoothSpecification Service => 
                BluetoothServices.BloodPressure;
        }
    }
### <a name="advanced_provider"></a> Creating an OAuth Resource Provider
TODO
### <a name="advanced_request"></a> Creating an OAuth Request
TODO
