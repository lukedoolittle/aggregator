# Quantfabric Material

## What is your usage scenario?
1. [I have a desktop or mobile app* and I want to use an OAuth1 authentication provider (ie Login with Twitter)](#one)
2. [I have a desktop or mobile app* and I want to use an OAuth2 authentication provider (ie Login with Facebook)](#two)
3. [I have a web application and I want to use an OAuth1 authentication provider (ie Login with Twitter)](#three)
4. [I have a web application and I want to use an OAuth2 authentication provider (ie Login with Facebook)](#four)
5. [I have a desktop, web or mobile app* and I want to access OAuth protected resources (ie access to my users Tweets)](#)
6. [I want to test OAuth2 authentication workflows for an OAuth2 provider I am creating](#two)

\*  in progress

## List of OAuth1 Providers
* `Twitter`
* `Fatsecret`
* `Withings`
* [Create a Provider](#advanced_provider)

## List of OAuth2 Providers
* `Facebook`
* `Fitbit`
* `Foursquare`
* `Google`
* `LinkedIn`
* `Rescuetime`
* `Runkeeper`
* `Spotify`
* `TwentyThreeAndMe`
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
* [Create a Request](#advanced_request)

## <a name="three"></a> Web App Authentication Provider (OAuth1)
To obtain the redirect url for authorization on the resource providers server:

	string consumerKey = "YOUR CONSUMER KEY";
	string callbackUri = "HTTP://YOURCALLBACKURI";
	
	OAuth1WebFacade<Twitter> oauth1 = new OAuth1WebFacade<Twitter>(
		consumerKey, 
		consumerSecret, 
		callbackUri);
			
	Uri twitterLoginEndpoint = await oauth1
		.GetAuthorizationUri()
		.ConfigureAwait(false);

To handle the callback to your server (assume this callback is an endpoint `TwitterCallback` within an .NET MVC `Controller`):

	public async Task<ActionResult> TwitterCallback()
	{
		Uri callbackUri = ControllerContext.HttpContext.Request.Url;
		OAuthCallbackHandler handler = new OAuthCallbackHandler();

		OAuth1Credentials intermediateCredentials = handler
			.ParseAndValidateCallback<OAuth1Credentials>(callbackUri);
	
		string consumerKey = "YOUR CONSUMER KEY";
		string consumerSecret = "YOUR CONSUMER SECRET";
		
		OAuth1WebFacade<Twitter> oauth1 = new OAuth1WebFacade<Twitter>(
			consumerKey, 
			consumerSecret, 
			callbackUri);
			
		OAuth1Credentials credentials = await oauth1
			.GetAccessTokenFromCallbackResult(intermediateCredentials)
			.ConfigureAwait(false);
		
		//you've got the credentials!

		return RedirectToAction("SOMEREDIRECTPAGE"); 
	}

## <a name="four"></a> Web App Authentication Provider (OAuth2)
To obtain the redirect url for authorization on the resource providers server:

	string clientId = "YOUR CLIENT ID";
	string clientSecret = "YOUR CLIENT SECRET";
	string callbackUri = "HTTP://YOURCALLBACKURI";
	string userId = "SOMEUSERID";  //some unique identifier stored in a cookie or session state
	
	OAuth2WebFacade<Facebook> oauth2 = new OAuth2WebFacade<Facebook>(
		clientId, 
		clientSecret, 
		userId,
		callbackUri);
			
	Uri facebookLoginEndpoint = await oauth2
		.GetAuthorizationUri()
		.ConfigureAwait(false);

If you want to make a subsequent request for a protected resource add scopes for each request you intend to make. For example:

	Uri facebookLoginEndpoint = await oauth2
		.AddScope<FacebookFeed>()
		.AddScope<FacebookFriend>()
		.GetAuthorizationUri()
		.ConfigureAwait(false);

To handle the callback to your server (assume this callback is an endpoint `FacebookCallback` within an .NET MVC `Controller` and that the userId is stored in a cookie):

	public async Task<ActionResult> FacebookCallback()
	{
		Uri callbackUri = ControllerContext.HttpContext.Request.Url;
		var userId = Request.Cookies["userId"];
		string clientId = "YOUR CLIENT ID";
		string clientSecret = "YOUR CLIENT SECRET";
		
		OAuth2WebFacade<Facebook> oauth2 = new OAuth2WebFacade<Facebook>(
			clientId, 
			clientSecret, 
			userId,
			callbackUri);

		OAuth2Credentials intermediateCredentials = oauth2
			.ParseAndValidateCallback(callbackUri);
		
		OAuth2Credentials credentials = await oauth2
			.GetAccessTokenFromCallbackResult(intermediateCredentials)
			.ConfigureAwait(false);
		
		//you've got the credentials!

		return RedirectToAction("SOMEREDIRECTPAGE"); 
	}

\* If you are working in an environment with multiple servers and you do not use sticky sessions, see the advanced topic Creating Your Own Security Strategy

## <a name="one"></a> Desktop/Mobile Authentication Provider (OAuth1)
When creating your app with the resource provider use your localhost as the callback uri with an uncommon port number. For example `http://localhost:33533/twitter`

	public async Task MyAuthenticationMethod()
	{
		string consumerKey = "YOUR CONSUMER KEY";
		string consumerSecret = "YOUR CONSUMER SECRET";
		string callbackUri = "http://localhost:33533/twitter";
		
		OAuth1AppFacade<Twitter> oauth1 = new OAuth1AppFacade<Twitter>(
			consumerKey, 
			consumerSecret, 
			callbackUri);
			
		OAuth1Credentials credentials = await oauth1
			.GetOAuth1Credentials()
			.ConfigureAwait(false);
	}

## <a name="two"></a> Desktop/Mobile Authentication Provider or for Testing (OAuth2)
When creating your app with the resource provider use your localhost as the callback uri with an uncommon port number. For example `http://localhost:33533/facebook`

	public async Task MyAuthenticationMethod()
	{
		string clientId = "YOUR CLIENT ID";
		string callbackUri = "http://localhost:33533/facebook";
		
		OAuth2AppFacade<Facebook> oauth2 = new OAuth2AppFacade<Facebook>(
			clientId, 
			callbackUri);
		
		OAuth2Credentials credentials = await oauth2
			.GetOAuth2Credentials()
			.ConfigureAwait(false);
	}

If you want to make a subsequent request for a protected resource add scopes for each request you intend to make. For example:

	OAuth2Credentials credentials = await oauth2
		.AddScope<FacebookFeed>()
		.AddScope<FacebookFriend>()
		.GetOAuth2Credentials()
		.ConfigureAwait(false);

The 'code' workflow can also be used in the event a long lived access token is desired and the client secret can be embedded in the application (testing purposes, etc)

	public async Task MyAuthenticationMethod()
	{
		string clientId = "YOUR CLIENT ID";
		string clientSecret = "YOUR CLIENT SECRET";
		string callbackUri = "HTTP://YOURCALLBACKURI";
		
		OAuth2AppFacade<Facebook> oauth2 = new OAuth2AppFacade<Facebook>(
			clientId, 
			clientSecret, 
			callbackUri);
		
		OAuth2Credentials credentials = await oauth2
			.GetOAuth2Credentials()
			.ConfigureAwait(false);
	}

## Accessing Protected Resources
After gathering the necessary `OAuth1Credentials` by using either the [OAuth 1 web](#two) or [OAuth 1 desktop](#one) workflows, or `OAuth2Credentials` by using either the [OAuth 2 web](#four) or [OAuth 2 desktop](#three) workflows, a request for a protected resource can be made.

	OAuth1Credentials twitterCredentials = CREDENTIALS_I_GOT_EARLIER;
    
    var response = await new OAuthRequester(twitterCredentials)
    	.MakeOAuthRequest<TwitterTweet, TwitterTweetResponse>()
    	.ConfigureAwait(false);

If the request needs to be customized an instance of the request class can be created

	OAuth1Credentials twitterCredentials = CREDENTIALS_I_GOT_EARLIER;
    
    var request = new TwitterTweet();
    request.Count = 100;
    var response = await new OAuthRequester(twitterCredentials)
    	.MakeOAuthRequest<TwitterTweet, TwitterTweetResponse>(request)
    	.ConfigureAwait(false);


## Advanced Topics
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
        
### <a name="advanced_provider"></a> Creating your own Resource Provider
TODO
### <a name="advanced_request"></a> Creating your own Request
TODO
