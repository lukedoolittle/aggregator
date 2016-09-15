Quantfabric.Material Readme

**IMPORTANT**
Android:
If you intend to use Bluetooth the BLUETOOTH, BLUETOOTH_ADMIN, BLUETOOTH_PRIVILEDGED permissions must be selected in your appmanifest
If you intend to use GPS the ACCESS_COARSE_LOCATION & ACCESS_FINE_LOCATION permissions must be selected in your appmanifest 
If you intend to use SMS the READ_SMS, READ_CONTACTS permissions must be selected in your appmanifest
If you intend to use OAuth the ACCESS_NETWORK_STATE, ACCESS_WIFI_STATE, INTERNET permissions must be selected in your appmanifest

iOS:
If you intend to use GPS the NSLocationAlwaysUsageDescription and UIBackgroundModes keys have to be added to your Info.plist
	<key>NSLocationAlwaysUsageDescription</key>
	<string></string>
	<key>UIBackgroundModes</key>
	<array>
		<string>location</string>
		<string>fetch</string>
	</array>
If you intend to use the Bluetooth the UIBackgroundModes key has to be added to your Info.plist
	<key>UIBackgroundModes</key>
	<array>
		<string>fetch</string>
		<string>bluetooth-central</string>
		<string>bluetooth-peripheral</string>
	</array>
	
UWP:
If you intend to use an OAuth workflow with a dedicated browser insert the following into your Package.appxmanifest (or simply add a protocol with the proper scheme through the GUI)

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