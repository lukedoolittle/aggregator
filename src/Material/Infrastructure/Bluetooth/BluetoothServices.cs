namespace Material.Infrastructure.Bluetooth
{
    //Reference
    //https://developer.bluetooth.org/gatt/services/Pages/ServiceViewer.aspx?u={SpecificationType}.xml
    public static class BluetoothServices
    {
        public static BluetoothSpecification AlertNotificationService { get; } = 
            new BluetoothSpecification("Alert Notification Service", "org.bluetooth.service.alert_notification", 0x1811);
        public static BluetoothSpecification AutomationIO { get; } = 
            new BluetoothSpecification("Automation IO", "org.bluetooth.service.automation_io", 0x1815);
        public static BluetoothSpecification BatteryService { get; } = 
            new BluetoothSpecification("Battery Service", "org.bluetooth.service.battery_service", 0x180F);
        public static BluetoothSpecification BloodPressure { get; } = 
            new BluetoothSpecification("Blood Pressure", "org.bluetooth.service.blood_pressure", 0x1810);
        public static BluetoothSpecification BodyComposition { get; } = 
            new BluetoothSpecification("Body Composition", "org.bluetooth.service.body_composition", 0x181B);
        public static BluetoothSpecification BondManagement { get; } = 
            new BluetoothSpecification("Bond Management", "org.bluetooth.service.bond_management", 0x181E);
        public static BluetoothSpecification ContinuousGlucoseMonitoring { get; } = 
            new BluetoothSpecification("Continuous Glucose Monitoring", "org.bluetooth.service.continuous_glucose_monitoring", 0x181F);
        public static BluetoothSpecification CurrentTimeService { get; } = 
            new BluetoothSpecification("Current Time Service", "org.bluetooth.service.current_time", 0x1805);
        public static BluetoothSpecification CyclingPower { get; } = 
            new BluetoothSpecification("Cycling Power", "org.bluetooth.service.cycling_power", 0x1818);
        public static BluetoothSpecification CyclingSpeedAndCadence { get; } = 
            new BluetoothSpecification("Cycling Speed and Cadence", "org.bluetooth.service.cycling_speed_and_cadence", 0x1816);
        public static BluetoothSpecification DeviceInformation { get; } = 
            new BluetoothSpecification("Device Information", "org.bluetooth.service.device_information", 0x180A);
        public static BluetoothSpecification EnvironmentalSensing { get; } = 
            new BluetoothSpecification("Environmental Sensing", "org.bluetooth.service.environmental_sensing", 0x181A);
        public static BluetoothSpecification GenericAccess { get; } = 
            new BluetoothSpecification("Generic Access", "org.bluetooth.service.generic_access", 0x1800);
        public static BluetoothSpecification GenericAttribute { get; } = 
            new BluetoothSpecification("Generic Attribute", "org.bluetooth.service.generic_attribute", 0x1801);
        public static BluetoothSpecification Glucose { get; } = 
            new BluetoothSpecification("Glucose", "org.bluetooth.service.glucose", 0x1808);
        public static BluetoothSpecification HealthThermometer { get; } = 
            new BluetoothSpecification("Health Thermometer", "org.bluetooth.service.health_thermometer", 0x1809);
        public static BluetoothSpecification HeartRate { get; } = 
            new BluetoothSpecification("Heart Rate", "org.bluetooth.service.heart_rate", 0x180D);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HTTP")]
        public static BluetoothSpecification HTTPProxy { get; } = 
            new BluetoothSpecification("HTTP Proxy", "org.bluetooth.service.http_proxy", 0x1823);
        public static BluetoothSpecification HumanInterfaceDevice { get; } = 
            new BluetoothSpecification("Human Interface Device", "org.bluetooth.service.human_interface_device", 0x1812);
        public static BluetoothSpecification ImmediateAlert { get; } = 
            new BluetoothSpecification("Immediate Alert", "org.bluetooth.service.immediate_alert", 0x1802);
        public static BluetoothSpecification IndoorPositioning { get; } = 
            new BluetoothSpecification("Indoor Positioning", "org.bluetooth.service.indoor_positioning", 0x1821);
        public static BluetoothSpecification InternetProtocolSupport { get; } = 
            new BluetoothSpecification("Internet Protocol Support", "org.bluetooth.service.internet_protocol_support", 0x1820);
        public static BluetoothSpecification LinkLoss { get; } = 
            new BluetoothSpecification("Link Loss", "org.bluetooth.service.link_loss", 0x1803);
        public static BluetoothSpecification LocationAndNavigation { get; } = 
            new BluetoothSpecification("Location and Navigation", "org.bluetooth.service.location_and_navigation", 0x1819);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DST")]
        public static BluetoothSpecification NextDSTChangeService { get; } = 
            new BluetoothSpecification("Next DST Change Service", "org.bluetooth.service.next_dst_change", 0x1807);
        public static BluetoothSpecification ObjectTransfer { get; } = 
            new BluetoothSpecification("Object Transfer", "org.bluetooth.service.object_transfer", 0x1825);
        public static BluetoothSpecification PhoneAlertStatusService { get; } = 
            new BluetoothSpecification("Phone Alert Status Service", "org.bluetooth.service.phone_alert_status", 0x180E);
        public static BluetoothSpecification PulseOximeter { get; } = 
            new BluetoothSpecification("Pulse Oximeter", "org.bluetooth.service.pulse_oximeter", 0x1822);
        public static BluetoothSpecification ReferenceTimeUpdateService { get; } = 
            new BluetoothSpecification("Reference Time Update Service", "org.bluetooth.service.reference_time_update", 0x1806);
        public static BluetoothSpecification RunningSpeedAndCadence { get; } = 
            new BluetoothSpecification("Running Speed and Cadence", "org.bluetooth.service.running_speed_and_cadence", 0x1814);
        public static BluetoothSpecification ScanParameters { get; } = 
            new BluetoothSpecification("Scan Parameters", "org.bluetooth.service.scan_parameters", 0x1813);
        public static BluetoothSpecification TransportDiscovery { get; } = 
            new BluetoothSpecification("Transport Discovery", "org.bluetooth.service.transport_discovery", 0x1824);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tx")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tx")]
        public static BluetoothSpecification TxPower { get; } = 
            new BluetoothSpecification("Tx Power", "org.bluetooth.service.tx_power", 0x1804);
        public static BluetoothSpecification UserData { get; } = 
            new BluetoothSpecification("User Data", "org.bluetooth.service.user_data", 0x181C);
        public static BluetoothSpecification WeightScale { get; } = 
            new BluetoothSpecification("Weight Scale", "org.bluetooth.service.weight_scale", 0x181D);

    }
}
