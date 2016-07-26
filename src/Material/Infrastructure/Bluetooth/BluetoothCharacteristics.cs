namespace Material.Infrastructure.Bluetooth
{
    //Reference
    //https://developer.bluetooth.org/gatt/characteristics/Pages/CharacteristicViewer.aspx?u={SpecificationType}.xml
    public static class BluetoothCharacteristics
    {
        public static BluetoothSpecification AerobicHeartRateLowerLimit =
            new BluetoothSpecification("Aerobic Heart Rate Lower Limit", "org.bluetooth.characteristic.aerobic_heart_rate_lower_limit", 0x2A7E);
        public static BluetoothSpecification AerobicHeartRateUpperLimit =
           new BluetoothSpecification("Aerobic Heart Rate Upper Limit", "org.bluetooth.characteristic.aerobic_heart_rate_upper_limit", 0x2A84);
        public static BluetoothSpecification AerobicThreshold =
           new BluetoothSpecification("Aerobic Threshold", "org.bluetooth.characteristic.aerobic_threshold", 0x2A7F);
        public static BluetoothSpecification Age =
           new BluetoothSpecification("Age", "org.bluetooth.characteristic.age", 0x2A80);
        public static BluetoothSpecification Aggregate =
           new BluetoothSpecification("Aggregate", "org.bluetooth.characteristic.aggregate", 0x2A5A);
        public static BluetoothSpecification AlertCategoryID =
           new BluetoothSpecification("Alert Category ID", "org.bluetooth.characteristic.alert_category_id", 0x2A43);
        public static BluetoothSpecification AlertCategoryIDBitMask =
           new BluetoothSpecification("Alert Category ID Bit Mask", "org.bluetooth.characteristic.alert_category_id_bit_mask", 0x2A42);
        public static BluetoothSpecification AlertLevel =
           new BluetoothSpecification("Alert Level", "org.bluetooth.characteristic.alert_level", 0x2A06);
        public static BluetoothSpecification AlertNotificationControlPoint =
           new BluetoothSpecification("Alert Notification Control Point", "org.bluetooth.characteristic.alert_notification_control_point", 0x2A44);
        public static BluetoothSpecification AlertStatus =
           new BluetoothSpecification("Alert Status", "org.bluetooth.characteristic.alert_status", 0x2A3F);
        public static BluetoothSpecification Altitude =
           new BluetoothSpecification("Altitude", "org.bluetooth.characteristic.altitude", 0x2AB3);
        public static BluetoothSpecification AnaerobicHeartRateLowerLimit =
           new BluetoothSpecification("Anaerobic Heart Rate Lower Limit", "org.bluetooth.characteristic.anaerobic_heart_rate_lower_limit", 0x2A81);
        public static BluetoothSpecification AnaerobicHeartRateUpperLimit =
           new BluetoothSpecification("Anaerobic Heart Rate Upper Limit", "org.bluetooth.characteristic.anaerobic_heart_rate_upper_limit", 0x2A82);
        public static BluetoothSpecification AnaerobicThreshold =
           new BluetoothSpecification("Anaerobic Threshold", "org.bluetooth.characteristic.anaerobic_threshold", 0x2A83);
        public static BluetoothSpecification Analog =
           new BluetoothSpecification("Analog", "org.bluetooth.characteristic.analog", 0x2A58);
        public static BluetoothSpecification ApparentWindDirection =
           new BluetoothSpecification("Apparent Wind Direction ", "org.bluetooth.characteristic.apparent_wind_direction", 0x2A73);
        public static BluetoothSpecification ApparentWindSpeed =
           new BluetoothSpecification("Apparent Wind Speed", "org.bluetooth.characteristic.apparent_wind_speed", 0x2A72);
        public static BluetoothSpecification Appearance =
           new BluetoothSpecification("Appearance", "org.bluetooth.characteristic.gap.appearance", 0x2A01);
        public static BluetoothSpecification BarometricPressureTrend =
           new BluetoothSpecification("Barometric Pressure Trend", "org.bluetooth.characteristic.barometric_pressure_trend", 0x2AA3);
        public static BluetoothSpecification BatteryLevel =
           new BluetoothSpecification("Battery Level", "org.bluetooth.characteristic.battery_level", 0x2A19);
        public static BluetoothSpecification BloodPressureFeature =
           new BluetoothSpecification("Blood Pressure Feature", "org.bluetooth.characteristic.blood_pressure_feature", 0x2A49);
        public static BluetoothSpecification BloodPressureMeasurement =
           new BluetoothSpecification("Blood Pressure Measurement", "org.bluetooth.characteristic.blood_pressure_measurement", 0x2A35);
        public static BluetoothSpecification BodyCompositionFeature =
           new BluetoothSpecification("Body Composition Feature", "org.bluetooth.characteristic.body_composition_feature", 0x2A9B);
        public static BluetoothSpecification BodyCompositionMeasurement =
           new BluetoothSpecification("Body Composition Measurement", "org.bluetooth.characteristic.body_composition_measurement", 0x2A9C);
        public static BluetoothSpecification BodySensorLocation =
           new BluetoothSpecification("Body Sensor Location", "org.bluetooth.characteristic.body_sensor_location", 0x2A38);
        public static BluetoothSpecification BondManagementControlPoint =
           new BluetoothSpecification("Bond Management Control Point", "org.bluetooth.characteristic.bond_management_control_point", 0x2AA4);
        public static BluetoothSpecification BondManagementFeature =
           new BluetoothSpecification("Bond Management Feature", "org.bluetooth.characteristic.bond_management_feature", 0x2AA5);
        public static BluetoothSpecification BootKeyboardInputReport =
           new BluetoothSpecification("Boot Keyboard Input Report", "org.bluetooth.characteristic.boot_keyboard_input_report", 0x2A22);
        public static BluetoothSpecification BootKeyboardOutputReport =
           new BluetoothSpecification("Boot Keyboard Output Report", "org.bluetooth.characteristic.boot_keyboard_output_report", 0x2A32);
        public static BluetoothSpecification BootMouseInputReport =
           new BluetoothSpecification("Boot Mouse Input Report", "org.bluetooth.characteristic.boot_mouse_input_report", 0x2A33);
        public static BluetoothSpecification CentralAddressResolution =
           new BluetoothSpecification("Central Address Resolution", "org.bluetooth.characteristic.gap.central_address_resolution_support", 0x2AA6);
        public static BluetoothSpecification CGMFeature =
           new BluetoothSpecification("CGM Feature", "org.bluetooth.characteristic.cgm_feature", 0x2AA8);
        public static BluetoothSpecification CGMMeasurement =
           new BluetoothSpecification("CGM Measurement", "org.bluetooth.characteristic.cgm_measurement", 0x2AA7);
        public static BluetoothSpecification CGMSessionRunTime =
           new BluetoothSpecification("CGM Session Run Time", "org.bluetooth.characteristic.cgm_session_run_time", 0x2AAB);
        public static BluetoothSpecification CGMSessionStartTime =
           new BluetoothSpecification("CGM Session Start Time", "org.bluetooth.characteristic.cgm_session_start_time", 0x2AAA);
        public static BluetoothSpecification CGMSpecificOpsControlPoint =
           new BluetoothSpecification("CGM Specific Ops Control Point", "org.bluetooth.characteristic.cgm_specific_ops_control_point", 0x2AAC);
        public static BluetoothSpecification CGMStatus =
           new BluetoothSpecification("CGM Status", "org.bluetooth.characteristic.cgm_status", 0x2AA9);
        public static BluetoothSpecification CSCFeature =
           new BluetoothSpecification("CSC Feature", "org.bluetooth.characteristic.csc_feature", 0x2A5C);
        public static BluetoothSpecification CSCMeasurement =
           new BluetoothSpecification("CSC Measurement", "org.bluetooth.characteristic.csc_measurement", 0x2A5B);
        public static BluetoothSpecification CurrentTime =
           new BluetoothSpecification("Current Time", "org.bluetooth.characteristic.current_time", 0x2A2B);
        public static BluetoothSpecification CyclingPowerControlPoint =
           new BluetoothSpecification("Cycling Power Control Point", "org.bluetooth.characteristic.cycling_power_control_point", 0x2A66);
        public static BluetoothSpecification CyclingPowerFeature =
           new BluetoothSpecification("Cycling Power Feature", "org.bluetooth.characteristic.cycling_power_feature", 0x2A65);
        public static BluetoothSpecification CyclingPowerMeasurement =
           new BluetoothSpecification("Cycling Power Measurement", "org.bluetooth.characteristic.cycling_power_measurement", 0x2A63);
        public static BluetoothSpecification CyclingPowerVector =
           new BluetoothSpecification("Cycling Power Vector", "org.bluetooth.characteristic.cycling_power_vector", 0x2A64);
        public static BluetoothSpecification DatabaseChangeIncrement =
           new BluetoothSpecification("Database Change Increment", "org.bluetooth.characteristic.database_change_increment", 0x2A99);
        public static BluetoothSpecification DateofBirth =
           new BluetoothSpecification("Date of Birth", "org.bluetooth.characteristic.date_of_birth", 0x2A85);
        public static BluetoothSpecification DateofThresholdAssessment =
           new BluetoothSpecification("Date of Threshold Assessment", "org.bluetooth.characteristic.date_of_threshold_assessment", 0x2A86);
        public static BluetoothSpecification DateTime =
           new BluetoothSpecification("Date Time", "org.bluetooth.characteristic.date_time", 0x2A08);
        public static BluetoothSpecification DayDateTime =
           new BluetoothSpecification("Day Date Time", "org.bluetooth.characteristic.day_date_time", 0x2A0A);
        public static BluetoothSpecification DayofWeek =
           new BluetoothSpecification("Day of Week", "org.bluetooth.characteristic.day_of_week", 0x2A09);
        public static BluetoothSpecification DescriptorValueChanged =
           new BluetoothSpecification("Descriptor Value Changed", "org.bluetooth.characteristic.descriptor_value_changed", 0x2A7D);
        public static BluetoothSpecification DeviceName =
           new BluetoothSpecification("Device Name", "org.bluetooth.characteristic.gap.device_name", 0x2A00);
        public static BluetoothSpecification DewPoint =
           new BluetoothSpecification("Dew Point", "org.bluetooth.characteristic.dew_point", 0x2A7B);
        public static BluetoothSpecification Digital =
           new BluetoothSpecification("Digital", "org.bluetooth.characteristic.digital", 0x2A56);
        public static BluetoothSpecification DSTOffset =
           new BluetoothSpecification("DST Offset", "org.bluetooth.characteristic.dst_offset", 0x2A0D);
        public static BluetoothSpecification Elevation =
           new BluetoothSpecification("Elevation", "org.bluetooth.characteristic.elevation", 0x2A6C);
        public static BluetoothSpecification EmailAddress =
           new BluetoothSpecification("Email Address", "org.bluetooth.characteristic.email_address", 0x2A87);
        public static BluetoothSpecification ExactTime256 =
           new BluetoothSpecification("Exact Time 256", "org.bluetooth.characteristic.exact_time_256", 0x2A0C);
        public static BluetoothSpecification FatBurnHeartRateLowerLimit =
           new BluetoothSpecification("Fat Burn Heart Rate Lower Limit", "org.bluetooth.characteristic.fat_burn_heart_rate_lower_limit", 0x2A88);
        public static BluetoothSpecification FatBurnHeartRateUpperLimit =
           new BluetoothSpecification("Fat Burn Heart Rate Upper Limit", "org.bluetooth.characteristic.fat_burn_heart_rate_upper_limit", 0x2A89);
        public static BluetoothSpecification FirmwareRevisionString =
           new BluetoothSpecification("Firmware Revision String", "org.bluetooth.characteristic.firmware_revision_string", 0x2A26);
        public static BluetoothSpecification FirstName =
           new BluetoothSpecification("First Name", "org.bluetooth.characteristic.first_name", 0x2A8A);
        public static BluetoothSpecification FiveZoneHeartRateLimits =
           new BluetoothSpecification("Five Zone Heart Rate Limits", "org.bluetooth.characteristic.five_zone_heart_rate_limits", 0x2A8B);
        public static BluetoothSpecification FloorNumber =
           new BluetoothSpecification("Floor Number", "org.bluetooth.characteristic.floor_number", 0x2AB2);
        public static BluetoothSpecification Gender =
           new BluetoothSpecification("Gender", "org.bluetooth.characteristic.gender", 0x2A8C);
        public static BluetoothSpecification GlucoseFeature =
           new BluetoothSpecification("Glucose Feature", "org.bluetooth.characteristic.glucose_feature", 0x2A51);
        public static BluetoothSpecification GlucoseMeasurement =
           new BluetoothSpecification("Glucose Measurement", "org.bluetooth.characteristic.glucose_measurement", 0x2A18);
        public static BluetoothSpecification GlucoseMeasurementContext =
           new BluetoothSpecification("Glucose Measurement Context", "org.bluetooth.characteristic.glucose_measurement_context", 0x2A34);
        public static BluetoothSpecification GustFactor =
           new BluetoothSpecification("Gust Factor", "org.bluetooth.characteristic.gust_factor", 0x2A74);
        public static BluetoothSpecification HardwareRevisionString =
           new BluetoothSpecification("Hardware Revision String", "org.bluetooth.characteristic.hardware_revision_string", 0x2A27);
        public static BluetoothSpecification HeartRateControlPoint =
           new BluetoothSpecification("Heart Rate Control Point", "org.bluetooth.characteristic.heart_rate_control_point", 0x2A39);
        public static BluetoothSpecification HeartRateMax =
           new BluetoothSpecification("Heart Rate Max", "org.bluetooth.characteristic.heart_rate_max", 0x2A8D);
        public static BluetoothSpecification HeartRateMeasurement =
           new BluetoothSpecification("Heart Rate Measurement", "org.bluetooth.characteristic.heart_rate_measurement", 0x2A37);
        public static BluetoothSpecification HeatIndex =
           new BluetoothSpecification("Heat Index", "org.bluetooth.characteristic.heat_index", 0x2A7A);
        public static BluetoothSpecification Height =
           new BluetoothSpecification("Height", "org.bluetooth.characteristic.height", 0x2A8E);
        public static BluetoothSpecification HIDControlPoint =
           new BluetoothSpecification("HID Control Point", "org.bluetooth.characteristic.hid_control_point", 0x2A4C);
        public static BluetoothSpecification HIDInformation =
           new BluetoothSpecification("HID Information", "org.bluetooth.characteristic.hid_information", 0x2A4A);
        public static BluetoothSpecification HipCircumference =
           new BluetoothSpecification("Hip Circumference", "org.bluetooth.characteristic.hip_circumference", 0x2A8F);
        public static BluetoothSpecification HTTPControlPoint =
           new BluetoothSpecification("HTTP Control Point", "org.bluetooth.characteristic.http_control_point", 0x2ABA);
        public static BluetoothSpecification HTTPEntityBody =
           new BluetoothSpecification("HTTP Entity Body", "org.bluetooth.characteristic.http_entity_body", 0x2AB9);
        public static BluetoothSpecification HTTPHeaders =
           new BluetoothSpecification("HTTP Headers", "org.bluetooth.characteristic.http_headers", 0x2AB7);
        public static BluetoothSpecification HTTPStatusCode =
           new BluetoothSpecification("HTTP Status Code", "org.bluetooth.characteristic.http_status_code", 0x2AB8);
        public static BluetoothSpecification HTTPSSecurity =
           new BluetoothSpecification("HTTPS Security", "org.bluetooth.characteristic.https_security", 0x2ABB);
        public static BluetoothSpecification Humidity =
           new BluetoothSpecification("Humidity", "org.bluetooth.characteristic.humidity", 0x2A6F);
        public static BluetoothSpecification IEEE11073_20601RegulatoryCertificationDataList =
           new BluetoothSpecification("IEEE 11073-20601 Regulatory Certification Data List", "org.bluetooth.characteristic.ieee_11073-20601_regulatory_certification_data_list", 0x2A2A);
        public static BluetoothSpecification IndoorPositioningConfiguration =
           new BluetoothSpecification("Indoor Positioning Configuration", "org.bluetooth.characteristic.indoor_positioning_configuration", 0x2AAD);
        public static BluetoothSpecification IntermediateCuffPressure =
           new BluetoothSpecification("Intermediate Cuff Pressure", "org.bluetooth.characteristic.intermediate_cuff_pressure", 0x2A36);
        public static BluetoothSpecification IntermediateTemperature =
           new BluetoothSpecification("Intermediate Temperature", "org.bluetooth.characteristic.intermediate_temperature", 0x2A1E);
        public static BluetoothSpecification Irradiance =
           new BluetoothSpecification("Irradiance", "org.bluetooth.characteristic.irradiance", 0x2A77);
        public static BluetoothSpecification Language =
           new BluetoothSpecification("Language", "org.bluetooth.characteristic.language", 0x2AA2);
        public static BluetoothSpecification LastName =
           new BluetoothSpecification("Last Name", "org.bluetooth.characteristic.last_name", 0x2A90);
        public static BluetoothSpecification Latitude =
           new BluetoothSpecification("Latitude", "org.bluetooth.characteristic.latitude", 0x2AAE);
        public static BluetoothSpecification LNControlPoint =
           new BluetoothSpecification("LN Control Point", "org.bluetooth.characteristic.ln_control_point", 0x2A6B);
        public static BluetoothSpecification LNFeature =
           new BluetoothSpecification("LN Feature", "org.bluetooth.characteristic.ln_feature", 0x2A6A);
        public static BluetoothSpecification LocalEastCoordinate =
           new BluetoothSpecification("Local East Coordinate", "org.bluetooth.characteristic.local_east_coordinate", 0x2AB1);
        public static BluetoothSpecification LocalNorthCoordinate =
           new BluetoothSpecification("Local North Coordinate", "org.bluetooth.characteristic.local_north_coordinate", 0x2AB0);
        public static BluetoothSpecification LocalTimeInformation =
           new BluetoothSpecification("Local Time Information", "org.bluetooth.characteristic.local_time_information", 0x2A0F);
        public static BluetoothSpecification LocationandSpeed =
           new BluetoothSpecification("Location and Speed", "org.bluetooth.characteristic.location_and_speed", 0x2A67);
        public static BluetoothSpecification LocationName =
           new BluetoothSpecification("Location Name", "org.bluetooth.characteristic.location_name", 0x2AB5);
        public static BluetoothSpecification Longitude =
           new BluetoothSpecification("Longitude", "org.bluetooth.characteristic.longitude", 0x2AAF);
        public static BluetoothSpecification MagneticDeclination =
           new BluetoothSpecification("Magnetic Declination", "org.bluetooth.characteristic.magnetic_declination", 0x2A2C);
        public static BluetoothSpecification MagneticFluxDensity_2D =
           new BluetoothSpecification("Magnetic Flux Density - 2D", "org.bluetooth.characteristic.magnetic_flux_density_2D", 0x2AA0);
        public static BluetoothSpecification MagneticFluxDensity_3D =
           new BluetoothSpecification("Magnetic Flux Density - 3D", "org.bluetooth.characteristic.magnetic_flux_density_3D", 0x2AA1);
        public static BluetoothSpecification ManufacturerNameString =
           new BluetoothSpecification("Manufacturer Name String", "org.bluetooth.characteristic.manufacturer_name_string", 0x2A29);
        public static BluetoothSpecification MaximumRecommendedHeartRate =
           new BluetoothSpecification("Maximum Recommended Heart Rate", "org.bluetooth.characteristic.maximum_recommended_heart_rate", 0x2A91);
        public static BluetoothSpecification MeasurementInterval =
           new BluetoothSpecification("Measurement Interval", "org.bluetooth.characteristic.measurement_interval", 0x2A21);
        public static BluetoothSpecification ModelNumberString =
           new BluetoothSpecification("Model Number String", "org.bluetooth.characteristic.model_number_string", 0x2A24);
        public static BluetoothSpecification Navigation =
           new BluetoothSpecification("Navigation", "org.bluetooth.characteristic.navigation", 0x2A68);
        public static BluetoothSpecification NewAlert =
           new BluetoothSpecification("New Alert", "org.bluetooth.characteristic.new_alert", 0x2A46);
        public static BluetoothSpecification ObjectActionControlPoint =
           new BluetoothSpecification("Object Action Control Point", "org.bluetooth.characteristic.object_action_control_point", 0x2AC5);
        public static BluetoothSpecification ObjectChanged =
           new BluetoothSpecification("Object Changed", "org.bluetooth.characteristic.object_changed", 0x2AC8);
        public static BluetoothSpecification ObjectFirst_Created =
           new BluetoothSpecification("Object First-Created", "org.bluetooth.characteristic.object_first_created", 0x2AC1);
        public static BluetoothSpecification ObjectID =
           new BluetoothSpecification("Object ID", "org.bluetooth.characteristic.object_id", 0x2AC3);
        public static BluetoothSpecification ObjectLast_Modified =
           new BluetoothSpecification("Object Last-Modified", "org.bluetooth.characteristic.object_last_modified", 0x2AC2);
        public static BluetoothSpecification ObjectListControlPoint =
           new BluetoothSpecification("Object List Control Point", "org.bluetooth.characteristic.object_list_control_point", 0x2AC6);
        public static BluetoothSpecification ObjectListFilter =
           new BluetoothSpecification("Object List Filter", "org.bluetooth.characteristic.object_list_filter", 0x2AC7);
        public static BluetoothSpecification ObjectName =
           new BluetoothSpecification("Object Name", "org.bluetooth.characteristic.object_name", 0x2ABE);
        public static BluetoothSpecification ObjectProperties =
           new BluetoothSpecification("Object Properties", "org.bluetooth.characteristic.object_properties", 0x2AC4);
        public static BluetoothSpecification ObjectSize =
           new BluetoothSpecification("Object Size", "org.bluetooth.characteristic.object_size", 0x2AC0);
        public static BluetoothSpecification ObjectType =
           new BluetoothSpecification("Object Type", "org.bluetooth.characteristic.object_type", 0x2ABF);
        public static BluetoothSpecification OTSFeature =
           new BluetoothSpecification("OTS Feature", "org.bluetooth.characteristic.ots_feature", 0x2ABD);
        public static BluetoothSpecification PeripheralPreferredConnectionParameters =
           new BluetoothSpecification("Peripheral Preferred Connection Parameters", "org.bluetooth.characteristic.gap.peripheral_preferred_connection_parameters", 0x2A04);
        public static BluetoothSpecification PeripheralPrivacyFlag =
           new BluetoothSpecification("Peripheral Privacy Flag", "org.bluetooth.characteristic.gap.peripheral_privacy_flag", 0x2A02);
        public static BluetoothSpecification PLXContinuousMeasurement =
           new BluetoothSpecification("PLX Continuous Measurement", "org.bluetooth.characteristic.plx_continuous_measurement", 0x2A5F);
        public static BluetoothSpecification PLXFeatures =
           new BluetoothSpecification("PLX Features", "org.bluetooth.characteristic.plx_features", 0x2A60);
        public static BluetoothSpecification PLXSpot_CheckMeasurement =
           new BluetoothSpecification("PLX Spot-Check Measurement", "org.bluetooth.characteristic.plx_spot_check_measurement", 0x2A5E);
        public static BluetoothSpecification PnPID =
           new BluetoothSpecification("PnP ID", "org.bluetooth.characteristic.pnp_id", 0x2A50);
        public static BluetoothSpecification PollenConcentration =
           new BluetoothSpecification("Pollen Concentration", "org.bluetooth.characteristic.pollen_concentration", 0x2A75);
        public static BluetoothSpecification PositionQuality =
           new BluetoothSpecification("Position Quality", "org.bluetooth.characteristic.position_quality", 0x2A69);
        public static BluetoothSpecification Pressure =
           new BluetoothSpecification("Pressure", "org.bluetooth.characteristic.pressure", 0x2A6D);
        public static BluetoothSpecification ProtocolMode =
           new BluetoothSpecification("Protocol Mode", "org.bluetooth.characteristic.protocol_mode", 0x2A4E);
        public static BluetoothSpecification Rainfall =
           new BluetoothSpecification("Rainfall", "org.bluetooth.characteristic.rainfall", 0x2A78);
        public static BluetoothSpecification ReconnectionAddress =
           new BluetoothSpecification("Reconnection Address", "org.bluetooth.characteristic.gap.reconnection_address", 0x2A03);
        public static BluetoothSpecification RecordAccessControlPoint =
           new BluetoothSpecification("Record Access Control Point", "org.bluetooth.characteristic.record_access_control_point", 0x2A52);
        public static BluetoothSpecification ReferenceTimeInformation =
           new BluetoothSpecification("Reference Time Information", "org.bluetooth.characteristic.reference_time_information", 0x2A14);
        public static BluetoothSpecification Report =
           new BluetoothSpecification("Report", "org.bluetooth.characteristic.report", 0x2A4D);
        public static BluetoothSpecification ReportMap =
           new BluetoothSpecification("Report Map", "org.bluetooth.characteristic.report_map", 0x2A4B);
        public static BluetoothSpecification RestingHeartRate =
           new BluetoothSpecification("Resting Heart Rate", "org.bluetooth.characteristic.resting_heart_rate", 0x2A92);
        public static BluetoothSpecification RingerControlPoint =
           new BluetoothSpecification("Ringer Control Point", "org.bluetooth.characteristic.ringer_control_point", 0x2A40);
        public static BluetoothSpecification RingerSetting =
           new BluetoothSpecification("Ringer Setting", "org.bluetooth.characteristic.ringer_setting", 0x2A41);
        public static BluetoothSpecification RSCFeature =
           new BluetoothSpecification("RSC Feature", "org.bluetooth.characteristic.rsc_feature", 0x2A54);
        public static BluetoothSpecification RSCMeasurement =
           new BluetoothSpecification("RSC Measurement", "org.bluetooth.characteristic.rsc_measurement", 0x2A53);
        public static BluetoothSpecification SCControlPoint =
           new BluetoothSpecification("SC Control Point", "org.bluetooth.characteristic.sc_control_point", 0x2A55);
        public static BluetoothSpecification ScanIntervalWindow =
           new BluetoothSpecification("Scan Interval Window", "org.bluetooth.characteristic.scan_interval_window", 0x2A4F);
        public static BluetoothSpecification ScanRefresh =
           new BluetoothSpecification("Scan Refresh", "org.bluetooth.characteristic.scan_refresh", 0x2A31);
        public static BluetoothSpecification SensorLocation =
           new BluetoothSpecification("Sensor Location", "org.blueooth.characteristic.sensor_location", 0x2A5D);
        public static BluetoothSpecification SerialNumberString =
           new BluetoothSpecification("Serial Number String", "org.bluetooth.characteristic.serial_number_string", 0x2A25);
        public static BluetoothSpecification ServiceChanged =
           new BluetoothSpecification("Service Changed", "org.bluetooth.characteristic.gatt.service_changed", 0x2A05);
        public static BluetoothSpecification SoftwareRevisionString =
           new BluetoothSpecification("Software Revision String", "org.bluetooth.characteristic.software_revision_string", 0x2A28);
        public static BluetoothSpecification SportTypeforAerobicandAnaerobicThresholds =
           new BluetoothSpecification("Sport Type for Aerobic and Anaerobic Thresholds", "org.bluetooth.characteristic.sport_type_for_aerobic_and_anaerobic_thresholds", 0x2A93);
        public static BluetoothSpecification SupportedNewAlertCategory =
           new BluetoothSpecification("Supported New Alert Category", "org.bluetooth.characteristic.supported_new_alert_category", 0x2A47);
        public static BluetoothSpecification SupportedUnreadAlertCategory =
           new BluetoothSpecification("Supported Unread Alert Category", "org.bluetooth.characteristic.supported_unread_alert_category", 0x2A48);
        public static BluetoothSpecification SystemID =
           new BluetoothSpecification("System ID", "org.bluetooth.characteristic.system_id", 0x2A23);
        public static BluetoothSpecification TDSControlPoint =
           new BluetoothSpecification("TDS Control Point", "org.bluetooth.characteristic.tds_control_point", 0x2ABC);
        public static BluetoothSpecification Temperature =
           new BluetoothSpecification("Temperature", "org.bluetooth.characteristic.temperature", 0x2A6E);
        public static BluetoothSpecification TemperatureMeasurement =
           new BluetoothSpecification("Temperature Measurement", "org.bluetooth.characteristic.temperature_measurement", 0x2A1C);
        public static BluetoothSpecification TemperatureType =
           new BluetoothSpecification("Temperature Type", "org.bluetooth.characteristic.temperature_type", 0x2A1D);
        public static BluetoothSpecification ThreeZoneHeartRateLimits =
           new BluetoothSpecification("Three Zone Heart Rate Limits", "org.bluetooth.characteristic.three_zone_heart_rate_limits", 0x2A94);
        public static BluetoothSpecification TimeAccuracy =
           new BluetoothSpecification("Time Accuracy", "org.bluetooth.characteristic.time_accuracy", 0x2A12);
        public static BluetoothSpecification TimeSource =
           new BluetoothSpecification("Time Source", "org.bluetooth.characteristic.time_source", 0x2A13);
        public static BluetoothSpecification TimeUpdateControlPoint =
           new BluetoothSpecification("Time Update Control Point", "org.bluetooth.characteristic.time_update_control_point", 0x2A16);
        public static BluetoothSpecification TimeUpdateState =
           new BluetoothSpecification("Time Update State", "org.bluetooth.characteristic.time_update_state", 0x2A17);
        public static BluetoothSpecification TimewithDST =
           new BluetoothSpecification("Time with DST", "org.bluetooth.characteristic.time_with_dst", 0x2A11);
        public static BluetoothSpecification TimeZone =
           new BluetoothSpecification("Time Zone", "org.bluetooth.characteristic.time_zone", 0x2A0E);
        public static BluetoothSpecification TrueWindDirection =
           new BluetoothSpecification("True Wind Direction", "org.bluetooth.characteristic.true_wind_direction", 0x2A71);
        public static BluetoothSpecification TrueWindSpeed =
           new BluetoothSpecification("True Wind Speed", "org.bluetooth.characteristic.true_wind_speed", 0x2A70);
        public static BluetoothSpecification TwoZoneHeartRateLimit =
           new BluetoothSpecification("Two Zone Heart Rate Limit", "org.bluetooth.characteristic.two_zone_heart_rate_limit", 0x2A95);
        public static BluetoothSpecification TxPowerLevel =
           new BluetoothSpecification("Tx Power Level", "org.bluetooth.characteristic.tx_power_level", 0x2A07);
        public static BluetoothSpecification Uncertainty =
           new BluetoothSpecification("Uncertainty", "org.bluetooth.characteristic.uncertainty", 0x2AB4);
        public static BluetoothSpecification UnreadAlertStatus =
           new BluetoothSpecification("Unread Alert Status", "org.bluetooth.characteristic.unread_alert_status", 0x2A45);
        public static BluetoothSpecification URI =
           new BluetoothSpecification("URI", "org.bluetooth.characteristic.uri", 0x2AB6);
        public static BluetoothSpecification UserControlPoint =
           new BluetoothSpecification("User Control Point", "org.bluetooth.characteristic.user_control_point", 0x2A9F);
        public static BluetoothSpecification UserIndex =
           new BluetoothSpecification("User Index", "org.bluetooth.characteristic.user_index", 0x2A9A);
        public static BluetoothSpecification UVIndex =
           new BluetoothSpecification("UV Index", "org.bluetooth.characteristic.uv_index", 0x2A76);
        public static BluetoothSpecification VO2Max =
           new BluetoothSpecification("VO2 Max", "org.bluetooth.characteristic.vo2_max", 0x2A96);
        public static BluetoothSpecification WaistCircumference =
           new BluetoothSpecification("Waist Circumference", "org.bluetooth.characteristic.waist_circumference", 0x2A97);
        public static BluetoothSpecification Weight =
           new BluetoothSpecification("Weight", "org.bluetooth.characteristic.weight", 0x2A98);
        public static BluetoothSpecification WeightMeasurement =
           new BluetoothSpecification("Weight Measurement", "org.bluetooth.characteristic.weight_measurement", 0x2A9D);
        public static BluetoothSpecification WeightScaleFeature =
           new BluetoothSpecification("Weight Scale Feature", "org.bluetooth.characteristic.weight_scale_feature", 0x2A9E);
        public static BluetoothSpecification WindChill =
           new BluetoothSpecification("Wind Chill", "org.bluetooth.characteristic.wind_chill", 0x2A79);

    }
}
