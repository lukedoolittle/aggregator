***Quantfabric.Material Readme***

-------------------------------
            Android
-------------------------------
If you intend to use Bluetooth the BLUETOOTH, BLUETOOTH_ADMIN, BLUETOOTH_PRIVILEDGED permissions must be selected in your appmanifest
If you intend to use GPS the ACCESS_COARSE_LOCATION & ACCESS_FINE_LOCATION permissions must be selected in your appmanifest 
If you intend to use SMS the READ_SMS, READ_CONTACTS permissions must be selected in your appmanifest
If you intend to use OAuth the ACCESS_NETWORK_STATE, ACCESS_WIFI_STATE, INTERNET permissions must be selected in your appmanifest

If you intend to use an OAuth workflow with a dedicated browser insert the following into your AndroidManifest.xml:
	<manifest>
		<application>
			<activity android:name="SOME_NAME">
				<intent-filter>
					<action android:name="android.intent.action.MAIN" />
					<category android:name="android.intent.category.LAUNCHER" />
				</intent-filter>
				<intent-filter>
					<action android:name="android.intent.action.VIEW" />
					<category android:name="android.intent.category.DEFAULT" />
					<category android:name="android.intent.category.BROWSABLE" />
					<data android:scheme="CALLBACK_SCHEME_HERE" />
				</intent-filter>
			</activity>
		</application>
	</manifest>
	
And decorate the main activity with the following metadata
	[Activity(Name = "SOME_NAME")]

-------------------------------
              iOS
-------------------------------
If you intend to use GPS the NSLocationAlwaysUsageDescription and UIBackgroundModes keys have to be added to your Info.plist:
	<plist>
		<dict>
			<key>NSLocationAlwaysUsageDescription</key>
			<string></string>
			<key>UIBackgroundModes</key>
			<array>
				<string>location</string>
				<string>fetch</string>
			</array>
		</dict>
	</plist>

If you intend to use the Bluetooth the UIBackgroundModes key has to be added to your Info.plist:
	<plist>
		<dict>
			<key>UIBackgroundModes</key>
			<array>
				<string>fetch</string>
				<string>bluetooth-central</string>
				<string>bluetooth-peripheral</string>
			</array>
		</dict>
	</plist>

If you intend to use an OAuth workflow with a dedicated browser insert the following into your Info.plist:
	<plist>
		<dict>
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
		</dict>
	</plist> 

-------------------------------
              UWP
-------------------------------
If you intend to use an OAuth workflow with a dedicated browser insert the following into your Package.appxmanifest (or simply add a protocol with the proper scheme through the GUI):
	<Package>
		<Applications>
			<Application>
				<Extensions>
					<uap:Extension Category="windows.protocol">
						<uap:Protocol Name="CALLBACK_SCHEME_HERE">
						</uap:Protocol>
					</uap:Extension>
				</Extensions>
			</Application>
		</Applications>
	</Package>

-------------------------------
     Xamarin.Forms (PCL)
-------------------------------
Make platform specific additions above AND add a dependency registration for the OAuthAuthorizerUIFactory and BluetoothAuthorizer to each platform project on launch

For Android (in MainActivity.OnCreate())

	protected override void OnCreate(Bundle bundle)
	{
		TabLayoutResource = Resource.Layout.Tabbar;
		ToolbarResource = Resource.Layout.Toolbar;

		base.OnCreate(bundle);

		global::Xamarin.Forms.Forms.Init(this, bundle);

		Xamarin.Forms.DependencyService.Register<OAuthAuthorizerUIFactory>();
		Xamarin.Forms.DependencyService.Register<BluetoothAuthorizerUIFactory>();

		LoadApplication(new App());
	}

For iOS (in AppDelegate.FinishLaunching())

	public override bool FinishedLaunching(UIApplication app, NSDictionary options)
	{
		global::Xamarin.Forms.Forms.Init();

		Xamarin.Forms.DependencyService.Register<OAuthAuthorizerUIFactory>();
		Xamarin.Forms.DependencyService.Register<BluetoothAuthorizerUIFactory>();

		LoadApplication(new App());

		return base.FinishedLaunching(app, options);
	}

For UWP (in App.OnLaunch())

	protected override void OnLaunched(LaunchActivatedEventArgs e)
	{
	    ...
		
		Xamarin.Forms.Forms.Init(e);
		
		Xamarin.Forms.DependencyService.Register<OAuthAuthorizerUIFactory>();
		
		...
	}