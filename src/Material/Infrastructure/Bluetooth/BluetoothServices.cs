namespace Material.Infrastructure.Bluetooth
{
    //Reference
    //https://developer.bluetooth.org/gatt/services/Pages/ServiceViewer.aspx?u={SpecificationType}.xml
    public static class BluetoothServices
    {
        public static BluetoothSpecification AlertNotificationService = 
            new BluetoothSpecification("Alert Notification Service", "org.bluetooth.service.alert_notification", 0x1811);
        public static BluetoothSpecification AutomationIO = 
            new BluetoothSpecification("Automation IO", "org.bluetooth.service.automation_io", 0x1815);
        public static BluetoothSpecification BatteryService = 
            new BluetoothSpecification("Battery Service", "org.bluetooth.service.battery_service", 0x180F);
        public static BluetoothSpecification BloodPressure = 
            new BluetoothSpecification("Blood Pressure", "org.bluetooth.service.blood_pressure", 0x1810);
        public static BluetoothSpecification BodyComposition = 
            new BluetoothSpecification("Body Composition", "org.bluetooth.service.body_composition", 0x181B);
        public static BluetoothSpecification BondManagement = 
            new BluetoothSpecification("Bond Management", "org.bluetooth.service.bond_management", 0x181E);
        public static BluetoothSpecification ContinuousGlucoseMonitoring = 
            new BluetoothSpecification("Continuous Glucose Monitoring", "org.bluetooth.service.continuous_glucose_monitoring", 0x181F);
        public static BluetoothSpecification CurrentTimeService = 
            new BluetoothSpecification("Current Time Service", "org.bluetooth.service.current_time", 0x1805);
        public static BluetoothSpecification CyclingPower = 
            new BluetoothSpecification("Cycling Power", "org.bluetooth.service.cycling_power", 0x1818);
        public static BluetoothSpecification CyclingSpeedandCadence = 
            new BluetoothSpecification("Cycling Speed and Cadence", "org.bluetooth.service.cycling_speed_and_cadence", 0x1816);
        public static BluetoothSpecification DeviceInformation = 
            new BluetoothSpecification("Device Information", "org.bluetooth.service.device_information", 0x180A);
        public static BluetoothSpecification EnvironmentalSensing = 
            new BluetoothSpecification("Environmental Sensing", "org.bluetooth.service.environmental_sensing", 0x181A);
        public static BluetoothSpecification GenericAccess = 
            new BluetoothSpecification("Generic Access", "org.bluetooth.service.generic_access", 0x1800);
        public static BluetoothSpecification GenericAttribute = 
            new BluetoothSpecification("Generic Attribute", "org.bluetooth.service.generic_attribute", 0x1801);
        public static BluetoothSpecification Glucose = 
            new BluetoothSpecification("Glucose", "org.bluetooth.service.glucose", 0x1808);
        public static BluetoothSpecification HealthThermometer = 
            new BluetoothSpecification("Health Thermometer", "org.bluetooth.service.health_thermometer", 0x1809);
        public static BluetoothSpecification HeartRate = 
            new BluetoothSpecification("Heart Rate", "org.bluetooth.service.heart_rate", 0x180D);
        public static BluetoothSpecification HTTPProxy = 
            new BluetoothSpecification("HTTP Proxy", "org.bluetooth.service.http_proxy", 0x1823);
        public static BluetoothSpecification HumanInterfaceDevice = 
            new BluetoothSpecification("Human Interface Device", "org.bluetooth.service.human_interface_device", 0x1812);
        public static BluetoothSpecification ImmediateAlert = 
            new BluetoothSpecification("Immediate Alert", "org.bluetooth.service.immediate_alert", 0x1802);
        public static BluetoothSpecification IndoorPositioning = 
            new BluetoothSpecification("Indoor Positioning", "org.bluetooth.service.indoor_positioning", 0x1821);
        public static BluetoothSpecification InternetProtocolSupport = 
            new BluetoothSpecification("Internet Protocol Support", "org.bluetooth.service.internet_protocol_support", 0x1820);
        public static BluetoothSpecification LinkLoss = 
            new BluetoothSpecification("Link Loss", "org.bluetooth.service.link_loss", 0x1803);
        public static BluetoothSpecification LocationandNavigation = 
            new BluetoothSpecification("Location and Navigation", "org.bluetooth.service.location_and_navigation", 0x1819);
        public static BluetoothSpecification NextDSTChangeService = 
            new BluetoothSpecification("Next DST Change Service", "org.bluetooth.service.next_dst_change", 0x1807);
        public static BluetoothSpecification ObjectTransfer = 
            new BluetoothSpecification("Object Transfer", "org.bluetooth.service.object_transfer", 0x1825);
        public static BluetoothSpecification PhoneAlertStatusService = 
            new BluetoothSpecification("Phone Alert Status Service", "org.bluetooth.service.phone_alert_status", 0x180E);
        public static BluetoothSpecification PulseOximeter = 
            new BluetoothSpecification("Pulse Oximeter", "org.bluetooth.service.pulse_oximeter", 0x1822);
        public static BluetoothSpecification ReferenceTimeUpdateService = 
            new BluetoothSpecification("Reference Time Update Service", "org.bluetooth.service.reference_time_update", 0x1806);
        public static BluetoothSpecification RunningSpeedandCadence = 
            new BluetoothSpecification("Running Speed and Cadence", "org.bluetooth.service.running_speed_and_cadence", 0x1814);
        public static BluetoothSpecification ScanParameters = 
            new BluetoothSpecification("Scan Parameters", "org.bluetooth.service.scan_parameters", 0x1813);
        public static BluetoothSpecification TransportDiscovery = 
            new BluetoothSpecification("Transport Discovery", "org.bluetooth.service.transport_discovery", 0x1824);
        public static BluetoothSpecification TxPower = 
            new BluetoothSpecification("Tx Power", "org.bluetooth.service.tx_power", 0x1804);
        public static BluetoothSpecification UserData = 
            new BluetoothSpecification("User Data", "org.bluetooth.service.user_data", 0x181C);
        public static BluetoothSpecification WeightScale = 
            new BluetoothSpecification("Weight Scale", "org.bluetooth.service.weight_scale", 0x181D);

    }
}
