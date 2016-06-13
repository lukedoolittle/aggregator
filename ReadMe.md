# Data Collection Framework for Xamarin

## Usage

1. Authenticate a service from an Activity (Android), a UIViewController (iOS), or whatever (Windows). Example for iOS from a UIViewController.

        var factory = ServiceLocator.Current.GetInstance<iOSAuthenticationTaskFactory>();
        await factory.GenerateTask<Twitter>(aggregateId, version, this).Execute();

2. Create a sensor. Data collection begins automagically.

        var sender = ServiceLocator.Current.GetInstance<ICommandSender>();
        var command = new CreateSensorCommand<TwitterTweets>(aggregateId, version);
        sender.Send(command);

3. Profit???

## Manual Usage (w/o domain model)

1. Get an authentication token. Example for Windows.

        var factory = ServiceLocator.Current.GetInstance<WindowsAuthenticationTaskFactory>();
        var token = await factory.GenerateOAuth1Task<Twitter>().GetAccessTokenCredentials();

2. Use token to make a request using a client

        var factory = ServiceLocator.Current.GetInstance<IClientFactory>();
        var data = await factory.CreateClient<TwitterTweets>(token).GetDataPoints();

## Building

Prerequisites

* Visual Studio 2015 + Xamarin
* T4 Tools Extension for VS2015
* [Couchbase for Windows](http://www.couchbase.com/nosql-databases/downloads?gclid=Cj0KEQjwtaexBRCohZOAoOPL88oBEiQAr96eSHjxp20MeldXqDbRkXHD59O9iTwpfBlLfKXk2hA5KxIaAqrh8P8HAQ) if you want to fiddle with the Windows based tests

Dependencies (enable autorestore Nuget packages in Visual Studio)

* Netwonsoft.Json
* Restsharp
* Autofac
* CouchbaseNetClient, Couchbase.Lite
* CommonServiceLocator
* Monkey.Robotics
* Xam.Plugin.Geolocator
* SimpleCQRS.Portable
* xUnit (testing)
* Lightmock (testing)

## Basic Setup

Before anything can happen you have to change the `CredentialApplicationSettings` class and fill in your clientId/clientSecret or consumerKey/consumerSecret for each service you want to use. See the advanced topics for how these entries affect the workflow.

## Basic iOS Setup

1. In your main iOS project add a reference to Aggregator.iOS

2. In the Entitlements.plist add the following: NSLocationAlwaysUsageDescription

3. Make your `AppDelegate` inherit from `AggregatorAppDelegate`

4. Basic usage is described at the begining of the readme


## Basic Android Setup

1. In your main Android project add a reference to Aggregator.Android

2. In your Android Manifest add the following permissions enabled: ACCESS_COARSE_LOCATION, ACCESS_FINE_LOCATION, BLUETOOTH, BLUETOOTH_ADMIN, BLUETOOTH_PRIVILEDGED, READ_SMS, READ_CONTACTS

3. Because the Android service starts asynchronously, in your initial activity (`MainLauncher=true`) the `OnCreate()` method should be marked `async` and wait for the startup to finish

        protected override async void OnCreate(Bundle bundle)
        {
            await AggregatorService.Initializing;
            ...
        }

4. Basic usage is described at the begining of the readme


## Basic Testing Setup

1. Change the testCredentials.txt file into testCredentials.json and fill in data with actual access/oauth tokens

2. Run all tests. (on Windows this will also run all the authentication tests)

3. Use the Android and iOS UI projects to manually test the authentication workflows


## Advanced Topics

### Client Credentials and Workflow

For OAuth2 authentication the workflow ('token' or 'code') is determined by the credentials you put into `CredentialApplicationSettings`. If you do not enter a client secret for a service type the authentication task will attempt to use the 'token' workflow, otherwise it will use the 'code' workflow

### Authentication Presentation Type

By default desktop applications attempt to use the default OS browser and mobile applications use an embedded view. If you want to override this you can provide a parameter when getting the authentication task.

    var factory = ServiceLocator.Current.GetInstance<AndroidAuthenticationTaskFactory>();
    var task = factory.GenerateTask<Fitbit>(aggregateId, version, this, AuthenticationInterfaceEnum.Dedicated);

This may be useful when a service requires something different than the default. For example Fitbit [forbids you from using a embedded view](https://dev.fitbit.com/docs/oauth2/#obtaining-consent) to authenticate. You could use the above code to force usage of the default Android browser for the authentication.

### Adding new EventHandlers

TODO

### Customizing the Bootstrapper

TODO

### Adding or removing Services or Requests

TODO